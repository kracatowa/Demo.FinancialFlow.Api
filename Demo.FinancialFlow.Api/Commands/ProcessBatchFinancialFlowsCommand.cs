using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessBatchFinancialFlowsCommand(List<Domain.FinancialFlowAggregate.FinancialFlow> financialFlowTransactions, Guid financialStorageId) : IRequest<Unit>
    {
        public List<Domain.FinancialFlowAggregate.FinancialFlow> FinancialFlowTransactions { get; } = financialFlowTransactions;
        public Guid FinancialStorageId { get; } = financialStorageId;
    }
}
