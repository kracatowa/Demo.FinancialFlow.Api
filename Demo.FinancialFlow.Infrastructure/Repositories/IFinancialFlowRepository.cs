using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Infrastructure.Repositories
{
    public interface IFinancialFlowRepository : IRepository
    {
        Task AddAsync(Domain.FinancialFlowAggregate.FinancialFlow financialFlow);
    }
}
