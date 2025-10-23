using Azure;
using Azure.Storage.Files.Shares;


namespace azure_storage
{
    internal class StorageService
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=pd311;AccountKey=eSLxVXV7vRcKujeukQnLxvrZbp3Qghtym60eu1kjUOMVTA76Xqlz54meRzF5NCzQ9JMluqUND/zW+AStQHCl6Q==;EndpointSuffix=core.windows.net";
        private const string shareName = "pd311";
        private const string directoryName = "images"; 
        private const string fileName = "joker.jpg";   
        private readonly ShareClient _shareClient;

        public StorageService()
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExistsAsync().Wait();
        }

        public async Task UploadFileAsync(string localFilePath)
        {
            if (!File.Exists(localFilePath))
                throw new FileNotFoundException($"{localFilePath} not found");

            var dirClient = _shareClient.GetDirectoryClient(directoryName);
            await dirClient.CreateIfNotExistsAsync();

            var fileClient = dirClient.GetFileClient(fileName);
            using var fileStream = File.OpenRead(localFilePath);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, fileStream.Length), fileStream);

            Console.WriteLine("File in Azure.");
        }

        public async Task DownloadFileAsync()
        {
            var dirClient = _shareClient.GetDirectoryClient(directoryName);
            if (!(await dirClient.ExistsAsync()).Value)
                throw new DirectoryNotFoundException($" {directoryName} not faund в Azure.");

            var fileClient = dirClient.GetFileClient(fileName);
            if (!(await fileClient.ExistsAsync()).Value)
                throw new FileNotFoundException($" {fileName}not faund {directoryName}.");

            var download = await fileClient.DownloadAsync();
            using var fileStream = File.Create($"downloaded-{fileName}");
            await download.Value.Content.CopyToAsync(fileStream);

            Console.WriteLine($"File in Azure in downloaded-{fileName}");
        }
    }
}
