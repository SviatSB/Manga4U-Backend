using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SERVICES.Extensions;

using SharedConfiguration.Options;

namespace WEBAPI.Extensions
{
    public static class JwtExtension
    {
        public static IServiceCollection AddAppJwt(this IServiceCollection services)
        {
            services
                .AddAppJwtBearer()
                .AddAppJwtGenerator();

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddAppJwtBearer(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .PostConfigure<IOptions<AppJwtOptions>>((jwtOptions, appJwt) =>
                {
                    var o = appJwt.Value;
                    var key = Encoding.UTF8.GetBytes(o.Key);

                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.FromMinutes(2),
                        ValidIssuer = o.Issuer,
                        ValidAudience = o.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            return services;
        }

    }
}
