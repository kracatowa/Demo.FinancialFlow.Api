namespace Demo.FinancialFlow.Domain.Seedwork
{
    public class Entity
    {
        public int Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected Entity()
        {
        }
    }
}
