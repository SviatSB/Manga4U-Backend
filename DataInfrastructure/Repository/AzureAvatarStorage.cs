using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using DataInfrastructure.Interfaces;

using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace DataInfrastructure.Repository
{
    public class AzureAvatarStorage : IAvatarStorage
    {
        private readonly BlobContainerClient _container;
        private readonly AppAzureStorageOptions _azureStorageOptions;

        public AzureAvatarStorage(BlobServiceClient blobServiceClient, IOptions<AppAzureStorageOptions> options)
        {
            _azureStorageOptions = options.Value;
            _container = blobServiceClient.GetBlobContainerClient(_azureStorageOptions.ContainerName);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            return blob.Uri.ToString();
        }
    }
}
