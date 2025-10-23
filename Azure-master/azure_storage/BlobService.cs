using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace azure_storage
{
    internal class BlobService
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=pd311;AccountKey=eSLxVXV7vRcKujeukQnLxvrZbp3Qghtym60eu1kjUOMVTA76Xqlz54meRzF5NCzQ9JMluqUND/zW+AStQHCl6Q==;EndpointSuffix=core.windows.net";

        public async Task UploadAsync(string containerName, string blobName, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{filePath} not found");

            var containerClient = new BlobContainerClient(connectionString, containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);

            using var fileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(fileStream, overwrite: true);

            Console.WriteLine($" Uploaded {blobName} to container {containerName}.");
        }
        //git
        public async Task DownloadAsync(string containerName, string blobName, string downloadPath)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!(await blobClient.ExistsAsync()).Value)
                throw new FileNotFoundException($"{blobName} not found in {containerName}");

            using var downloadStream = File.OpenWrite(downloadPath);
            await blobClient.DownloadToAsync(downloadStream);

            Console.WriteLine($" Downloaded {blobName} to {downloadPath}.");
        }

        //public async Task DeleteAsync(string containerName, string blobName)
        //{
        //    var containerClient = new BlobContainerClient(connectionString, containerName);
        //    var blobClient = containerClient.GetBlobClient(blobName);

        //    if (!(await blobClient.ExistsAsync()).Value)
        //        throw new FileNotFoundException($"{blobName} not found in {containerName}");

        //    await blobClient.DeleteAsync();

        //    Console.WriteLine($" Deleted {blobName} from {containerName}.");
        //}
    }
}
