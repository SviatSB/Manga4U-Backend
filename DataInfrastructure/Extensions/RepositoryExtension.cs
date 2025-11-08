using DataInfrastructure.Interfaces;
using DataInfrastructure.Repository;

using Microsoft.Extensions.DependencyInjection;

namespace DataInfrastructure.Extensions
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
