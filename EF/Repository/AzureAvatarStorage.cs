using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using ENTITIES.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAINFRASTRUCTURE.Repository
{
    public class AzureAvatarStorage : IAvatarStorage
    {
        private readonly BlobContainerClient _container;

        public AzureAvatarStorage(BlobServiceClient blobServiceClient, IConfiguration config)
        {
            var containerName = config["AzureStorage:ContainerName"];
            _container = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType});
            return  blob.Uri.ToString();
        }
    }
}
