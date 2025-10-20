using Demo.FinancialFlow.Domain.Seedwork;

namespace Demo.FinancialFlow.Domain
{
    public class FinancialFlow : Entity
    {
        public float Amount { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Description { get; private set; }
        public FlowType FlowType { get; private set; }
        public string Subsidiairy { get; private set; }

        public FinancialFlow(float amount, DateTime transactionDate, string description, FlowType flowType, string subsidiairy)
        {
            Amount = amount;
            TransactionDate = transactionDate;
            Description = description;
            FlowType = flowType;
            Subsidiairy = subsidiairy;
        }

        protected FinancialFlow() { }
    }

}
