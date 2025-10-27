namespace Demo.FinancialFlow.Api.Services.File
{
    public class FileProcessorFactory
    {
        private readonly IServiceProvider _provider;

        public FileProcessorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IFileProcessor GetProcessor(string extension)
        {
            return extension.ToLower() switch
            {
                ".csv" => _provider.GetRequiredService<ProcessCsvFinancialFlow>(),
                _ => throw new NotSupportedException($"File type not supported : {extension}")
            };
        }
    }
}
