using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

using ENTITIES.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace DATAINFRASTRUCTURE.Repository
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
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType});
            return  blob.Uri.ToString();
        }
    }
}
