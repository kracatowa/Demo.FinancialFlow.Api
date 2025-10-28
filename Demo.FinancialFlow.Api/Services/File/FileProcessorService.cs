namespace Demo.FinancialFlow.Api.Services.File
{
    public class FileProcessorService(FileProcessorFactory factory) : IFileProcessorService
    {
        public List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, string extension, Guid fileStorageId)
        {
            var processor = factory.GetProcessor(extension);
            return processor.ProcessFinancialFlow(stream, fileStorageId);
        }
    }
}
