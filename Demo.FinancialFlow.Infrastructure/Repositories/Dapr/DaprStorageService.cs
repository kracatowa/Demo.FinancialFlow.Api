using Dapr;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Dapr
{
    public class DaprStorageService(ILogger<DaprStorageService> logger, DaprClient daprClient, AsyncRetryPolicy retryPolicy) : IStorageService
    {
        public async Task<MemoryStream> Downloadfile(string fileName)
        {
            var bindingRequest = new BindingRequest("storageservice", "get");
            bindingRequest.Metadata.Add("blobName", fileName);

            try
            {
                var bindingResponse = await retryPolicy.ExecuteAsync(async () =>
                {
                    return await daprClient.InvokeBindingAsync(bindingRequest);
                }) ?? throw new ArgumentException($"File '{fileName}' not found in storage.");

                logger.LogInformation("fileBytes type: {Type}", bindingResponse?.GetType().FullName ?? "null");

                return new MemoryStream(bindingResponse.Data.ToArray());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to download file '{FileName}' after retries: {Message}", fileName, ex.Message);
                throw;
            }
        }

        public async void UploadFile(string fileName, Stream fileStream)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await fileStream.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }
            var metadata = new Dictionary<string, string> { ["blobName"] = fileName };

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
