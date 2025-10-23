using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Domain.FinancialFlowAggregate
{
    public interface IFinancialFlowRepository : IRepository
    {
        Task AddAsync(FinancialFlow financialFlow);
    }
}
