using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Demo.FinancialFlow.Infrastructure.Repositories;
using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessFinancialFlowFileCommandHandler(IFileProcessor fileProcessor,
                                                        IFinancialFlowRepository financialFlowRepository,
                                                        IStorageService storageService,
                                                        IMediator mediator) : IRequestHandler<ProcessFinancialFlowFileCommand, Unit>
    {
        public async Task<Unit> Handle(ProcessFinancialFlowFileCommand request, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream();

            var file = await storageService.Downloadfile(request.Filename);
            var financialFlows = fileProcessor.ProcessFinancialFlow(stream, request.Extension);

            foreach (var financialFlow in financialFlows)
            {
                await financialFlowRepository.AddAsync(financialFlow);
            }

            await financialFlowRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new FinancialFlowFileProcessedDomainEvent(request.FileStorageId), cancellationToken);

            return Unit.Value;
        }
    }
}
