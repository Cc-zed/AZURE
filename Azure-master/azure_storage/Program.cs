using System;
using System.Threading.Tasks;

namespace azure_storage
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var blobService = new BlobService();

            string containerName = "images";          // назва контейнера
            string blobName = "joker.jpg";            // ім’я в сховищі
            string localPath = "joker.jpg";           // шлях до локального файлу
            string downloadPath = "downloaded-joker.jpg"; // шлях для збереження завантаженого файлу

            try
            {
                await blobService.UploadAsync(containerName, blobName, localPath);
                await blobService.DownloadAsync(containerName, blobName, downloadPath);
                //await blobService.DeleteAsync(containerName, blobName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("End.");
        }
    }
}
