using Azure.Storage.Blobs;
using DATAINFRASTRUCTURE.Repository;
using ENTITIES.Interfaces;
using ENTITIES.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAINFRASTRUCTURE
{
    public static class DataInfrastructureDI
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, DataInfrastructureOptions options)
        {
            if (string.IsNullOrEmpty(options.DbConnectionString) || options.DbConnectionString == "InMemory")
            {
                services.AddDbContext<MyDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            }
            else
            {
                services.AddDbContext<MyDbContext>(o =>
                    o.UseSqlServer(options.DbConnectionString));
            }

            // ==Azure==
            if (string.IsNullOrEmpty(options.AzureStorageConnectionString))
                throw new ArgumentNullException(nameof(options.AzureStorageConnectionString));

            services.AddSingleton(provider =>
            {
                return new BlobServiceClient(options.AzureStorageConnectionString);
            });
            services.AddScoped<IAvatarStorage, AzureAvatarStorage>();
            // =========


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMangaRepository, MangaRepository>();
            services.AddScoped<IMangaRepository, CollectionRepository>();

            return services;
        }
    }
}
