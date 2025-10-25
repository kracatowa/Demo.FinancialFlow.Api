using Dapr;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Dapr
{
    public class DaprStorageService(ILogger<DaprStorageService> logger, DaprClient daprClient) : IStorageService
    {
        public async void UploadFile(string fileName, Stream fileStream)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await fileStream.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }
            var metadata = new Dictionary<string, string> { ["blobName"] = fileName };

            AsyncRetryPolicy retryPolicy = Policy
                .Handle<DaprException>()
                .Or<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        logger.LogWarning(exception, "Retry {RetryCount} for file '{FileName}' due to: {Message}", retryCount, fileName, exception.Message);
                    });

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    await daprClient.InvokeBindingAsync("storageservice", "create", fileBytes, metadata);
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to upload file '{FileName}' after retries: {Message}", fileName, ex.Message);
            }
        }
    }
}
