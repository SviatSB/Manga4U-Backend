using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENTITIES.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SERVICES.Services;

using SharedConfiguration.Options;

namespace SERVICES.Extensions
{
    public static class MyServicesExtension
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpClient<IMangaDexProxy, MangaDexProxy>("mangadex", (sp, c) =>
            {
                var proxyOptions = sp.GetRequiredService<IOptions<AppProxyOptions>>().Value;

                c.BaseAddress = new Uri("https://api.mangadex.org");
                c.DefaultRequestHeaders.Add("User-Agent", proxyOptions.UserAgent);
            });

            return services;
        }
    }
}
