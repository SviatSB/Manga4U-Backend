using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Options
{
    public class DataInfrastructureOptions
    {
        public string? DataBaseProvider { get; set; }
        public string? DbConnectionString { get; set; }
        public string? AzureStorageConnectionString { get; set; }
    }
}
