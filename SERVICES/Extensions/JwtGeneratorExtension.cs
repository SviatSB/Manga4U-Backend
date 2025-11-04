using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENTITIES.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using SERVICES.Services;

namespace SERVICES.Extensions
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
