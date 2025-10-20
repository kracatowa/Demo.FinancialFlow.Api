using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Infrastructure.Repositories;
using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessFinancialFlowFileCommandHandler(IFileProcessor fileProcessor,
                                                        IFinancialFlowRepository financialFlowRepository) : IRequestHandler<ProcessFinancialFlowFileCommand, Unit>
    {
        public async Task<Unit> Handle(ProcessFinancialFlowFileCommand request, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream, cancellationToken);

            var extension = Path.GetExtension(request.File.FileName);
            var financialFlows = fileProcessor.ProcessFinancialFlow(stream, extension);

            foreach(var financialFlow in financialFlows)
            {
                await financialFlowRepository.AddAsync(financialFlow);
            }
            
            await financialFlowRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
