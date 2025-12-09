using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Services.Interfaces;
using Services.Services;

using SharedConfiguration.Options;

namespace Services.Extensions
{
    public static class MyServicesExtension
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IMangaService, MangaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddHttpClient<IMangaDexService, MangaDexService>("mangadex", (sp, c) =>
            {
                var proxyOptions = sp.GetRequiredService<IOptions<AppProxyOptions>>().Value;

                c.BaseAddress = new Uri("https://api.mangadex.org");
                c.DefaultRequestHeaders.Add("User-Agent", proxyOptions.UserAgent);
            });
            services.AddScoped<IStatService, StatService>();

            return services;
        }
    }
}
