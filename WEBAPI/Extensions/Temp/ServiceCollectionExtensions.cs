using System.Security.Claims;
using System.Text;

using Azure.Storage.Blobs;

using DATAINFRASTRUCTURE;
using DATAINFRASTRUCTURE.Extensions;
using DATAINFRASTRUCTURE.Repository;

using ENTITIES.Interfaces;
using ENTITIES.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SERVICES.Services;

using SharedConfiguration.Options;

namespace WEBAPI.Extensions.Temp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppControllersAndSwagger(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }


        public static IServiceCollection AddAppIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<long>>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions<IdentityOptions>()
                .PostConfigure<IOptions<AppIdentityOptions>>((o, appIdentity) =>
                {
                    var options = appIdentity.Value;

                    o.Password.RequiredLength = options.PasswordRequiredLength;
                    o.Password.RequireNonAlphanumeric = options.PasswordRequireNonAlphanumeric;
                    o.Password.RequireUppercase = options.PasswordRequireUppercase;
                    o.Password.RequireLowercase = options.PasswordRequireLowercase;
                    o.Password.RequireDigit = options.PasswordRequireDigit;

                    o.User.RequireUniqueEmail = options.UserRequireUniqueEmail;
                });

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
        }
}
