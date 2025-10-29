using Azure.Storage.Blobs;

using DATAINFRASTRUCTURE.Repository;

using ENTITIES.Interfaces;
using ENTITIES.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DATAINFRASTRUCTURE
{
    public static class DataInfrastructureDI
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, DataInfrastructureOptions options)
        {
            // Select provider based on configuration
            var provider = options.DataBaseProvider?.ToLowerInvariant();
            if (provider == "inmemory" || string.IsNullOrWhiteSpace(provider))
            {
                services.AddDbContext<MyDbContext>(db => db.UseInMemoryDatabase("TestDb"));
            }
            else if (provider == "sqlite")
            {
                if (string.IsNullOrWhiteSpace(options.DbConnectionString))
                    throw new ArgumentNullException(nameof(options.DbConnectionString), "SQLite requires a connection string.");

                services.AddDbContext<MyDbContext>(db =>
                    db.UseSqlite(options.DbConnectionString));
            }
            else if (provider == "sqlserver")
            {
                if (string.IsNullOrWhiteSpace(options.DbConnectionString))
                    throw new ArgumentNullException(nameof(options.DbConnectionString), "SQL Server requires a connection string.");

                services.AddDbContext<MyDbContext>(db =>
                    db.UseSqlServer(options.DbConnectionString, sql =>
                    {
                        // Transient failure resiliency
                        sql.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: new[] { 40613 } // Database not available
                        );
                        // Give cold starts a bit more time
                        sql.CommandTimeout(60);
                    }));
            }
            else
            {
                throw new NotSupportedException($"Unknown database provider: {options.DataBaseProvider}");
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
