using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace DATAINFRASTRUCTURE
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            // Require appsettings.json from WEBAPI project (solution layout known)
            var efDir = Directory.GetCurrentDirectory();
            var webApiDir = Path.Combine(efDir, "..", "WEBAPI");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(webApiDir)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var provider = configuration["DataBaseConnection:DataBaseProvider"]?.ToLowerInvariant();
            var connectionString = configuration["DataBaseConnection:ConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            if (provider == "sqlite")
            {
                optionsBuilder.UseSqlite(connectionString, b =>
                {
                    // Direct migrations into the SqliteMigrations assembly
                    b.MigrationsAssembly("SqliteMigrations");
                });
            }
            else if (provider == "sqlserver")
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("DataBaseConnection:ConnectionString is missing in WEBAPI/appsettings.json");
                optionsBuilder.UseSqlServer(connectionString, sql =>
                {
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: new[] { 40613 }
                    );
                    sql.CommandTimeout(60);
                    // Direct migrations into the SqlServerMigrations assembly
                    sql.MigrationsAssembly("SqlServerMigrations");
                });
            }
            else // default to in-memory for design-time if unspecified
            {
                optionsBuilder.UseInMemoryDatabase("DesignTimeDb");
            }

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
