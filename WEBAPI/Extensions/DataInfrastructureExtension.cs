using DATAINFRASTRUCTURE.Extensions;

namespace WEBAPI.Extensions
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
