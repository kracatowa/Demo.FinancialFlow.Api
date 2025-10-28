namespace Demo.FinancialFlow.Api.Services.File
{
    public interface IFileProcessorService
    {
        List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, string extension, Guid fileStorageId);
    }
}
