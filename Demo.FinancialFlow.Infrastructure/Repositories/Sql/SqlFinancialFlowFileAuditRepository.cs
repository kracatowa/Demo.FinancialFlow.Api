using Demo.FinancialFlow.Domain.FileAggregate;
using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Sql
{
    public class SqlFinancialFlowFileAuditRepository(FinancialFlowContext sqlContext) : IFinancialFlowFileAuditRepository
    {
        public IUnitOfWork UnitOfWork => sqlContext;

        public async Task AddAsync(FinancialFlowFileAudit financialFlowFileAudit)
        {
            await sqlContext.AddAsync(financialFlowFileAudit);
        }
    }
}
