using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Infrastructure.Repositories;
using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class StartUploadFinancialFlowFileCommandHandler(IStorageService storageService, IFinancialFlowFileAuditRepository financialFlowFileAuditRepository) : IRequestHandler<StartUploadFinancialFlowFileCommand, bool>
    {
        public async Task<bool> Handle(StartUploadFinancialFlowFileCommand request, CancellationToken cancellationToken)
        {
            var storageFileId = Guid.NewGuid();

            var financialFlowFileAudit = new FinancialFlowFileAudit(request.UserId, request.File.FileName, storageFileId);

            try
            {
                storageService.UploadFile(storageFileId.ToString(), request.File.OpenReadStream());
            }
            catch (Exception)
            {
                financialFlowFileAudit.MarkAsFailed();

                return false;
            }
            finally
            {
                await financialFlowFileAuditRepository.AddAsync(financialFlowFileAudit);
            }

            await financialFlowFileAuditRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
