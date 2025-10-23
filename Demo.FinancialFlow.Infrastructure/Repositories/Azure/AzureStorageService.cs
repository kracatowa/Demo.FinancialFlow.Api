using Azure;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Azure
{
    public class AzureStorageService(IOptions<AzureStorageSettings> options) : IStorageService
    {
        private readonly AzureStorageSettings _settings = options.Value;

        public void UploadFile(string fileName, Stream fileStream)
        {
            try
            {
                var blobClientOptions = new BlobClientOptions
                {
                    Retry =
                    {
                        Mode = RetryMode.Exponential,
                        MaxRetries = 5,
                        Delay = TimeSpan.FromSeconds(1),
                        MaxDelay = TimeSpan.FromSeconds(10)
                    }
                };

                var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

                containerClient.CreateIfNotExists();

                var blobClient = containerClient.GetBlobClient(fileName);

                blobClient.Upload(fileStream, overwrite: false);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException($"Azure upload failed: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"IO error during upload: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error during blob upload: {ex.Message}", ex);
            }
        }
    }
}