using System;
using System.IO;

using DATAINFRASTRUCTURE;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using SharedConfiguration;

namespace SqliteMigrations
{
    public class SqliteDbContextFactory : BaseDbContextFactory
    {
        protected override void ConfigureProvider(DbContextOptionsBuilder<MyDbContext> builder, string connectionString)
        {
            builder.UseSqlite(connectionString, b =>
            {
                b.MigrationsAssembly("SqliteMigrations");
            });
        }
    }
}
