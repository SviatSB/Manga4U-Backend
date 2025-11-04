using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DATAINFRASTRUCTURE.Repository;

using ENTITIES.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace DATAINFRASTRUCTURE.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddAppRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMangaRepository, MangaRepository>();
            services.AddScoped<IMangaRepository, CollectionRepository>();

            return services;
        }
    }
}
