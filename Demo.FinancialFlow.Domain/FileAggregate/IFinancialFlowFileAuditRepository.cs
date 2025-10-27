using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Domain.FileAggregate
{
    public interface IFinancialFlowFileAuditRepository : IRepository
    {
        Task AddAsync(FinancialFlowFileAudit financialFlowFileAudit);
        Task<FinancialFlowFileAudit?> GetByStorageFileId(Guid id);
    }
}
