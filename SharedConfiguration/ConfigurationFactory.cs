using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace SharedConfiguration
{
    public static class ConfigurationFactory
    {
        public static IConfiguration BuildConfiguration(string? basePath = null)
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(ConfigurationFactory).Assembly.Location)!;
            basePath ??= assemblyLocation;

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("sharedsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return new AppConfiguration(config);
        }
    }
}
