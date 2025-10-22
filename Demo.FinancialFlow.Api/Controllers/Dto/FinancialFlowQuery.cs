using Demo.FinancialFlow.Domain.FinancialFlowAggregate;

namespace Demo.FinancialFlow.Api.Controllers.Dto
{
    public class FinancialFlowQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public float? MinAmount { get; set; }
        public float? MaxAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Description { get; set; }
        public FlowType? FlowType { get; set; }
        public string? Subsidiairy { get; set; }
    }
}
