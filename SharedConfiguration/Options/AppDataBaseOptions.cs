using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedConfiguration.Options
{
    public class AppDataBaseOptions
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
