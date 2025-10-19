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
        private const string FrontendCorsPolicy = "AllowFrontend";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // ===== Services =====
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ===== Database =====
            var conString = config.GetValue<bool>("Config:UseInMemoryDB")
                ? config.GetConnectionString("InMemoryConnection")
                : config.GetConnectionString("DefaultConnection");
            builder.Services.AddDataBaseDI(conString);

            // ===== Identity =====
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

            // ===== JWT =====
            var jwtConfig = new JwtConfig();
            config.GetSection("Jwt").Bind(jwtConfig);
            builder.Services.AddSingleton(jwtConfig);

            var key = Encoding.UTF8.GetBytes(jwtConfig.Key);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                // 🔍 JWT debug logging
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        Console.WriteLine($"[JWT] Auth failed: {ctx.Exception}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        Console.WriteLine($"[JWT] Challenge: {ctx.Error}, {ctx.ErrorDescription}");
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = ctx =>
                    {
                        var auth = ctx.Request.Headers["Authorization"].FirstOrDefault();
                        if (auth != null) Console.WriteLine($"[JWT] Header found: {auth[..Math.Min(auth.Length, 50)]}...");
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            // ===== CORS для фронтенду =====
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(FrontendCorsPolicy, policy =>
                {
                    policy
                        .WithOrigins(
                            "http://127.0.0.1:5500",
                            "https://127.0.0.1:5500",
                            "http://localhost:5500",
                            "https://localhost:5500"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddServicesDI();

            // ===== App pipeline =====
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            // Seed owner user
            var ownerLogin = config["Seed:OwnerLogin"];
            var ownerPassword = config["Seed:OwnerPassword"];
            using (var scope = app.Services.CreateScope())
            {
                SeedData.InitializeAsync(scope.ServiceProvider, ownerLogin, ownerPassword).GetAwaiter().GetResult();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // 🟢 ВАЖЛИВО: CORS перед аутентифікацією
            app.UseCors(FrontendCorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
