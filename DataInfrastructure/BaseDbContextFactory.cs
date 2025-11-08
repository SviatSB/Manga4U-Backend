using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SharedConfiguration;
using SharedConfiguration.Options;

namespace DataInfrastructure
{
    public abstract class BaseDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        protected abstract void ConfigureProvider(DbContextOptionsBuilder<MyDbContext> builder, string connectionString);

        public MyDbContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationFactory.BuildConfiguration();
            var wrappedConfig = new AppConfiguration(configuration);

            var dbOptions = wrappedConfig.GetOptions<AppDataBaseOptions>();

            var builder = new DbContextOptionsBuilder<MyDbContext>();
            ConfigureProvider(builder, dbOptions.ConnectionString);

            return new MyDbContext(builder.Options);
        }
    }
}
