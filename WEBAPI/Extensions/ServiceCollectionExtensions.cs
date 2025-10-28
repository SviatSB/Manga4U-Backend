using System.Security.Claims;
using System.Text;
using DATAINFRASTRUCTURE;
using ENTITIES;
using ENTITIES.Models;
using ENTITIES.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WEBAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Controllers + Swagger
        public static IServiceCollection AddAppControllersAndSwagger(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        // Database + Azure Storage
        public static IServiceCollection AddDatabaseAndAzureStorage(this IServiceCollection services, IConfiguration config)
        {
            var dbConString = config.GetValue<bool>("Config:UseInMemoryDB")
            ? config.GetConnectionString("InMemoryConnection")
            : config.GetConnectionString("DefaultConnection");

            var azureStorageConString = config.GetValue<string>("AzureStorage:ConnectionString");

            services.AddDataInfrastructure(
            new DataInfrastructureOptions
            {
                DbConnectionString = dbConString,
                AzureStorageConnectionString = azureStorageConString
            });

            return services;
        }

        // Identity + JWT (same logic as in Program.cs)
        public static IServiceCollection AddIdentityAndJwt(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<User, IdentityRole<long>>(options =>
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

            var jwtConfig = new JwtConfig();
            config.GetSection("Jwt").Bind(jwtConfig);
            services.AddSingleton(jwtConfig);

            var key = Encoding.UTF8.GetBytes(jwtConfig.Key);
            services.AddAuthentication(options =>
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

                // Debug logging as-is
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

            services.AddAuthorization();

            return services;
        }

        // CORS
        public static IServiceCollection AddFrontendCors(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy
                         .SetIsOriginAllowed(_ => true) //TODO избавиться от этого
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials();
                });
            });
            return services;
        }
    }
}
