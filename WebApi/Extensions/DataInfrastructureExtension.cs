using DataInfrastructure.Extensions;

namespace WebApi.Extensions
{
    public static class DataInfrastructureExtension
    {
        public static IServiceCollection AddAppDataInfrastructure(this IServiceCollection services)
        {
            services
                .AddAppDataBase()
                .AddAppRepository()
                .AddAppAzureStorage();

            return services;
        }
    }
}
