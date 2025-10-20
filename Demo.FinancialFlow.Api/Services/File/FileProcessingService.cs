namespace Demo.FinancialFlow.Api.Services.File
{
    public class FileProcessingService : IFileProcessor
    {
        private readonly FileProcessorFactory _factory;

        public FileProcessingService(FileProcessorFactory factory)
        {
            _factory = factory;
        }

        public List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, string extension)
        {
            var processor = _factory.GetProcessor(extension);
            return processor.ProcessFinancialFlow(stream, extension);
        }
    }
}
