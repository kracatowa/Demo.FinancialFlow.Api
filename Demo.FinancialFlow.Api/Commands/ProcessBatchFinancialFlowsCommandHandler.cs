using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessBatchFinancialFlowsCommandHandler(IFinancialFlowRepository financialFlowRepository,
                                                        IMediator mediator) : IRequestHandler<ProcessBatchFinancialFlowsCommand, Unit>
    {
        public async Task<Unit> Handle(ProcessBatchFinancialFlowsCommand request, CancellationToken cancellationToken)
        {
            foreach (var financialFlow in request.FinancialFlowTransactions)
            {
                await financialFlowRepository.AddAsync(financialFlow);
            }

            await financialFlowRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new FinancialFlowFileProcessedDomainEvent(request.FinancialStorageId), cancellationToken);

            return Unit.Value;
        }
    }
}
