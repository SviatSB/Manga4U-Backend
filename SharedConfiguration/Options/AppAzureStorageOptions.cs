using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedConfiguration.Options
{
    public class AppAzureStorageOptions
    {
        public string ConnectionString { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
        public string DefaultAvatarUrl { get; set; } = null!;

    }
}
