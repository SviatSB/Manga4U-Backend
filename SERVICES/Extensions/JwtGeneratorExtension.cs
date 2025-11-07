using Microsoft.Extensions.DependencyInjection;

using Services.Interfaces;

namespace Services.Extensions
{
    public static class JwtGeneratorExtension
    {
        public static IServiceCollection AddAppJwtGenerator(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, IJwtTokenGenerator>();
            return services;
        }
    }
}
