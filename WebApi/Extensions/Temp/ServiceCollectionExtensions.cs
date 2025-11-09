using DataInfrastructure;

using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace WebApi.Extensions.Temp
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

        //todo перенести в datainfrstructure
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
