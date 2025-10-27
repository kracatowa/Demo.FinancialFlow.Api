using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Sql
{
    public class SqlFinancialFlowFileAuditRepository(FinancialFlowContext sqlContext) : IFinancialFlowFileAuditRepository
    {
        public IUnitOfWork UnitOfWork => sqlContext;

        public async Task AddAsync(FinancialFlowFileAudit financialFlowFileAudit)
        {
            await sqlContext.AddAsync(financialFlowFileAudit);
        }

        public async Task<FinancialFlowFileAudit?> GetByStorageFileId(Guid id)
        {
            return await sqlContext.FinancialFlowFileAudits.FirstOrDefaultAsync(x=>x.StorageFileId == id);
        }
    }
}
