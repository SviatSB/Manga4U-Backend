using Azure.Storage.Blobs;

using DATAINFRASTRUCTURE.Repository;

using ENTITIES.Interfaces;
using ENTITIES.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SharedConfiguration.Options;

namespace DATAINFRASTRUCTURE
{
    public static class DataInfrastructureDI
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, AppDataBaseOptions options)
        {
            // Select provider based on configuration
            var provider = options.Provider;

            if (provider == ProviderType.InMemory)
            {
                services.AddDbContext<MyDbContext>(db => db.UseInMemoryDatabase("TestDb"));
            }
            else if (provider == ProviderType.Sqlite)
            {
                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                    throw new ArgumentNullException(nameof(options.ConnectionString), "SQLite requires a connection string.");

                services.AddDbContext<MyDbContext>(db =>
                    db.UseSqlite(options.ConnectionString));
            }
            else if (provider == ProviderType.SqlServer)
            {
                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                    throw new ArgumentNullException(nameof(options.ConnectionString), "SQL Server requires a connection string.");

                services.AddDbContext<MyDbContext>(db =>
                    db.UseSqlServer(options.ConnectionString, sql =>
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
                throw new NotSupportedException($"Unknown database provider: {options.Provider}");
            }

            return services;
        }
    }
}
