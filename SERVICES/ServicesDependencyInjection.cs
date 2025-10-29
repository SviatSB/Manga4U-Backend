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
using Microsoft.Extensions.Http;

namespace SERVICES
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpClient<IMangaDexProxy, MangaDexProxy>("mangadex", c =>
            {
                c.BaseAddress = new Uri("https://api.mangadex.org");
            });

            return services;
        }
    }
}
