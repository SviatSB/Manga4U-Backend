using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace DataInfrastructure.Extensions
{
    public static class DataBaseExtension
    {
        public static IServiceCollection AddAppDataBase(this IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>((provider, db) =>
            {
                var options = provider.GetRequiredService<IOptions<AppDataBaseOptions>>().Value;

                switch (options.Provider)
                {
                    case ProviderType.InMemory:
                        db.UseInMemoryDatabase("TestDb");
                        break;

                    case ProviderType.Sqlite:
                        ConfigureSqlite(db, options.ConnectionString);
                        break;

                    case ProviderType.SqlServer:
                        ConfigureSqlServer(db, options.ConnectionString);
                        break;

                    default:
                        throw new NotSupportedException($"Unknown database provider: {options.Provider}");
                }
            });

            return services;
        }
        private static void ConfigureSqlServer(DbContextOptionsBuilder db, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "SQL Server requires a connection string.");

            db.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: new[] { 40613 });
                sql.CommandTimeout(60);
            });
        }

        private static void ConfigureSqlite(DbContextOptionsBuilder db, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "SQLite requires a connection string.");

            db.UseSqlite(connectionString);
        }

    }
}
