using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharedConfiguration.Options;

namespace SharedConfiguration
{
    public static class AddConfiguration
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            // Bind all options used via IOptions<T>
            services
                .Configure<AppJwtOptions>(config.GetSection("JwtOptions"))
                .Configure<AppAzureStorageOptions>(config.GetSection("AzureStorageOptions"))
                .Configure<AppSeedOptions>(config.GetSection("SeedOptions"))
                .Configure<AppCacheOptions>(config.GetSection("CacheOptions"))
                .Configure<AppDataBaseOptions>(config.GetSection("DataBaseOptions"))
                .Configure<AppIdentityOptions>(config.GetSection("IdentityOptions"))
                .Configure<AppProxyOptions>(config.GetSection("ProxyOptions"));

            return services;
        }
    }
}
