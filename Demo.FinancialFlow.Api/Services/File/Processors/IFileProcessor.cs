namespace Demo.FinancialFlow.Api.Services.File.Processors
{
    public interface IFileProcessor
    {
        List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, Guid fileStorageId);
    }
}
