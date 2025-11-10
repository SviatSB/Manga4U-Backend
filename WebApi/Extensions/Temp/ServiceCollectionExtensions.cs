using DataInfrastructure;

using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using SharedConfiguration.Options;

namespace WebApi.Extensions.Temp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppControllersAndSwagger(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manga4U API", Version = "v1" });
                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token. Example: Bearer {token}",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };
                c.AddSecurityDefinition("Bearer", jwtScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtScheme, Array.Empty<string>() }
                });
            });
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
