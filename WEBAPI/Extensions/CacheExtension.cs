using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace WEBAPI.Extensions
{
    public static class CacheExtension
    {
        public static IServiceCollection AddAppCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddOptions<MemoryCacheEntryOptions>()
                .PostConfigure<IOptions<AppCacheOptions>>((opts, appCache) =>
                {
                    var o = appCache.Value;
                    opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(o.AbsoluteExpirationRelativeToNow);
                    opts.SlidingExpiration = TimeSpan.FromSeconds(o.SlidingExpiration);
                    opts.Priority = o.Priority;
                });

            return services;
        }
    }
}
