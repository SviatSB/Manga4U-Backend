using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppDataBaseOptions : IAppOptions
    {
        public string ConnectionString { get; set; } = null!;
        public ProviderType Provider { get; set; } = ProviderType.Sqlite;
    }

    public enum ProviderType
    {
        Sqlite = 1,
        SqlServer = 2,
        InMemory = 3
    }
}
