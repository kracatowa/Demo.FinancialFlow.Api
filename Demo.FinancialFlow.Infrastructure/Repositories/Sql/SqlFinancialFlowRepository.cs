using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Infrastructure.Repositories.Sql
{
    public class SqlFinancialFlowRepository(FinancialFlowContext sqlContext) : IFinancialFlowRepository
    {
        public IUnitOfWork UnitOfWork => sqlContext;

        public async Task AddAsync(Domain.FinancialFlowAggregate.FinancialFlow financialFlow)
        {
            await sqlContext.AddAsync(financialFlow);
        }
    }
}
