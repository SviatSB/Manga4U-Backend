using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

using Azure.Storage.Blobs;

using DATAINFRASTRUCTURE;
using DATAINFRASTRUCTURE.Repository;

using ENTITIES;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using ENTITIES.Options;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

using SERVICES.Services;

using SharedConfiguration.Options;

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
        public static IServiceCollection AddAppAzureStorage(this IServiceCollection services,  AppAzureStorageOptions options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new ArgumentNullException(nameof(options.ConnectionString));

            services.AddSingleton(provider =>
            {
                return new BlobServiceClient(options.ConnectionString);
            });
            services.AddScoped<IAvatarStorage, AzureAvatarStorage>();

            return services;
        }
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, AppDataBaseOptions options)
        {
            services.AddDataInfrastructure(options);

            return services;
        }

        public static IServiceCollection AddAppIdentity(this IServiceCollection services, AppIdentityOptions options)
        {
            services.AddIdentity<User, IdentityRole<long>>(o =>
            {
                o.Password.RequiredLength = options.PasswordRequiredLength;
                o.Password.RequireNonAlphanumeric = options.PasswordRequireNonAlphanumeric;
                o.Password.RequireUppercase = options.PasswordRequireUppercase;
                o.Password.RequireLowercase = options.PasswordRequireLowercase;
                o.Password.RequireDigit = options.PasswordRequireDigit;
                o.User.RequireUniqueEmail = options.UserRequireUniqueEmail;
            })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
        public static IServiceCollection AddAppJwt(this IServiceCollection services, AppJwtOptions options)
        {
            var key = Encoding.UTF8.GetBytes(options.Key);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                // Debug logging as-is
                o.Events = new JwtBearerEvents
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

        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton(provider =>
            {
                var o = provider.GetRequiredService<IOptions<AppCacheOptions>>().Value;
                return new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(o.AbsoluteExpirationRelativeToNow),
                    SlidingExpiration = TimeSpan.FromSeconds(o.SlidingExpiration),
                    Priority = o.Priority
                };
            });

            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMangaRepository, MangaRepository>();
            services.AddScoped<IMangaRepository, CollectionRepository>();

            return services;
        }

        public static IServiceCollection AddMyServices(this IServiceCollection services, AppProxyOptions options)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpClient<IMangaDexProxy, MangaDexProxy>("mangadex", c =>
            {
                c.BaseAddress = new Uri("https://api.mangadex.org");
                c.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
            });

            return services;
        }
    }
}
