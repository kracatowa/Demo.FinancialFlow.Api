using Demo.FinancialFlow.Api.Services.File.Processors;

namespace Demo.FinancialFlow.Api.Services.File
{
    public class FileProcessorFactory(IServiceProvider provider)
    {
        public IFileProcessor GetProcessor(string extension)
        {
            return extension.ToLower() switch
            {
                ".csv" => provider.GetRequiredService<ProcessCsvFinancialFlow>(),
                _ => throw new NotSupportedException($"File type not supported : {extension}")
            };
        }
    }
}
