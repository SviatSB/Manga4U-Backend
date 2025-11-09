using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Services.Interfaces;
using Services.Services.Seeders;

namespace Services.Extensions
{
    public static class SeedersExtension
    {
        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            //не синглтон, потому что там внутри исползьуется dbcontext и так будет проще
            services.AddScoped<ISeeder, TagSeeder>();
            services.AddScoped<ISeeder, IdentitySeeder>();

            return services;
        }
    }
}
