

using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Infrastructure.Repositories
{
    public class SqlFinancialFlowRepository(FinancialFlowContext context) : IFinancialFlowRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task AddAsync(Domain.FinancialFlowAggregate.FinancialFlow financialFlow)
        {
            await context.AddAsync(financialFlow);
        }
    }
}
