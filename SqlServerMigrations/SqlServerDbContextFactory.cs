using DataInfrastructure;

using Microsoft.EntityFrameworkCore;

namespace SqlServerMigrations
{
    public class SqlServerDbContextFactory : BaseDbContextFactory
    {
        protected override void ConfigureProvider(DbContextOptionsBuilder<MyDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), new[] { 40613 });
                sql.CommandTimeout(60);
                sql.MigrationsAssembly("SqlServerMigrations");
            });
        }
    }
}
