using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blobpractice.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IEnumerable<string>> AllBlobs(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var files = new List<string>();

            var blobs = containerClient.GetBlobsAsync();

            await foreach(var item in blobs)
            {
                files.Add(item.Name);
            }

            return files;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var res = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

            if(res != null)
            {
                return true;
            }

            return false;
        }
    }
}
