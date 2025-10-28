using System;
using System.IO;
using DATAINFRASTRUCTURE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SqlServerMigrations
{
    // Design-time factory dedicated to SQL Server migrations
    public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var webApiDir = Path.Combine(currentDir, "..", "WEBAPI");

            var configuration = new ConfigurationBuilder()
            .SetBasePath(webApiDir)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

            var connectionString = configuration["DataBaseConnection:ConnectionString"];
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), new[] { 40613 });
                sql.CommandTimeout(60);
                sql.MigrationsAssembly("SqlServerMigrations");
            });

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
