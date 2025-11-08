using DataInfrastructure;

using Microsoft.EntityFrameworkCore;

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
