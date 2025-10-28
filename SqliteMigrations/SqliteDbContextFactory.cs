using System;
using System.IO;
using DATAINFRASTRUCTURE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SqliteMigrations
{
    // Design-time factory dedicated to SQLite migrations
    public class SqliteDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
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
            optionsBuilder.UseSqlite(connectionString, b =>
            {
                b.MigrationsAssembly("SqliteMigrations");
            });
            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
