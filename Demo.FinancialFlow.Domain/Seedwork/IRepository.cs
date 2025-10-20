namespace Demo.FinancialFlow.Domain.Seedwork
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
