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

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing in WEBAPI/appsettings.json");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: new[] { 40613 }
                );
                sql.CommandTimeout(60);
            });

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
