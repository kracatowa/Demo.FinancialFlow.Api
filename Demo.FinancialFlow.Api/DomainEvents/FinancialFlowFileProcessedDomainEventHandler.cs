using Demo.FinancialFlow.Domain.FileAggregate;
using MediatR;

namespace Demo.FinancialFlow.Api.DomainEvents
{
    public class FinancialFlowFileProcessedHandler : INotificationHandler<FinancialFlowFileProcessedDomainEvent>
    {
        private readonly IFinancialFlowFileAuditRepository _repository;
        public FinancialFlowFileProcessedHandler(IFinancialFlowFileAuditRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(FinancialFlowFileProcessedDomainEvent notification, CancellationToken cancellationToken)
        {
            var file = await _repository.GetByStorageFileId(notification.FinancialFlowFileId) 
                        ?? throw new ArgumentException($"Financial flow file with storage id {notification.FinancialFlowFileId} not found.");

            file.MarkAsProcessed();

            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
