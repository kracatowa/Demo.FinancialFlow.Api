namespace Demo.FinancialFlow.Domain.FinancialFlowAggregate.FileDto
{
    public class CsvFinancialFlow
    {
        public float Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public required string Description { get; set; }
        public required string FlowType { get; set; }
        public required string Subsidiairy { get; set; }
        public FinancialFlow ToDomainModel(Guid fileStorageId)
        {
            return new FinancialFlow(
                Amount,
                TransactionDate,
                Description,
                Enum.Parse<FlowType>(FlowType),
                Subsidiairy,
                fileStorageId
            );
        }

        public IEnumerable<string> Validate()
        {
            if (Amount < 0)
                yield return "Amount must be non-negative.";
            if (string.IsNullOrWhiteSpace(Description))
                yield return "Description is required.";
            if (!Enum.TryParse<FlowType>(FlowType, true, out _))
                yield return $"FlowType '{FlowType}' is invalid.";
            if (string.IsNullOrWhiteSpace(Subsidiairy))
                yield return "Subsidiairy is required.";
            // Add more rules as needed
        }
    }
}
