using DATAINFRASTRUCTURE;
using ENTITIES.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //==--==--==--==--==

            string conString;
            if (config.GetValue<bool>("Config:UseInMemoryDB"))
            {
                conString = config.GetConnectionString("InMemoryConnection");
            }
            else
            {
                conString = config.GetConnectionString("DefaultConnection");
            }
            builder.Services.DataBaseDI(conString);

            //identity (по хорошему это должно быть в SERVICES методом расширением)
            builder.Services.AddIdentity<User, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

            //JWT
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();

            //==--==--==--==--==

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //==--==--==--==--==

            var ownerLogin = config["Seed:OwnerLogin"];
            var ownerPassword = config["Seed:OwnerPassword"];

            using (var scope = app.Services.CreateScope())
            {
                SeedData.InitializeAsync(scope.ServiceProvider, ownerLogin, ownerPassword).GetAwaiter().GetResult();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //==--==--==--==--==

            app.MapControllers();

            app.Run();
        }
    }
}
