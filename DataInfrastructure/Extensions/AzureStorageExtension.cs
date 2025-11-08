using Azure.Storage.Blobs;

using DataInfrastructure.Interfaces;
using DataInfrastructure.Repository;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace DataInfrastructure.Extensions
{
    public static class AzureStorageExtension
    {
        public static IServiceCollection AddAppAzureStorage(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppAzureStorageOptions>>().Value;
                return new BlobServiceClient(options.ConnectionString);
            });
            services.AddScoped<IAvatarStorage, AzureAvatarStorage>();

            return services;
        }
    }
}
