namespace Demo.FinancialFlow.Api.Services.File
{
    public interface IFileProcessor
    {
        List<Domain.FinancialFlowAggregate.FinancialFlow> ProcessFinancialFlow(MemoryStream stream, string extension);
    }
}
