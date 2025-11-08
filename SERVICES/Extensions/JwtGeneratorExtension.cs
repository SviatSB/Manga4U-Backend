using Microsoft.Extensions.DependencyInjection;

using Services.Interfaces;
using Services.Services;

namespace Services.Extensions
{
    public static class JwtGeneratorExtension
    {
        public static IServiceCollection AddAppJwtGenerator(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
