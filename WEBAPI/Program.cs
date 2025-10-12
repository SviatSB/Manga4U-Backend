using DATAINFRASTRUCTURE;
using ENTITIES;
using ENTITIES.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SERVICES;
using System.Text;
using System.Security.Claims;

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
            builder.Services.AddDataBaseDI(conString);

            //identity (по хорошему это должно быть в SERVICES методом расширением)
            builder.Services.AddIdentity<User, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

            //JWT

            var jwtConfig = new JwtConfig();
            builder.Configuration.GetSection("Jwt").Bind(jwtConfig);
            builder.Services.AddSingleton(jwtConfig);

            var key = Encoding.UTF8.GetBytes(jwtConfig.Key);
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    // Allow small clock drift to avoid immediate nbf/iat based failures
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        Console.WriteLine($"Token failed: {ctx.Exception}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        Console.WriteLine($"Challenge: {ctx.Error}, {ctx.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddServicesDI();


            //==--==--==--==--==

            var app = builder.Build();

            //Configure the HTTP request pipeline.
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //==--==--==--==--==

            app.MapControllers();

            app.Run();
        }
    }
}
