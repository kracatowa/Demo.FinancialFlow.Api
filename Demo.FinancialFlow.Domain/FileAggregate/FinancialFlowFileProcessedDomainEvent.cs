using MediatR;

namespace Demo.FinancialFlow.Domain.FileAggregate
{
    public class FinancialFlowFileProcessedDomainEvent(Guid financialFlowFileId) : INotification
    {
        public Guid FinancialFlowFileId { get; } = financialFlowFileId;
    }
}
