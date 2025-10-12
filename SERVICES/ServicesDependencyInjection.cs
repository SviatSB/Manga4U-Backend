using DATAINFRASTRUCTURE;
using DATAINFRASTRUCTURE.Repository;
using ENTITIES.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SERVICES.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddServicesDI(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
