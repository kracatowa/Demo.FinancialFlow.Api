using Demo.FinancialFlow.Domain.FinancialFlowAggregate;

namespace Demo.FinancialFlow.Api.Controllers.Dto
{
    public class FinancialFlowFileQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Filename { get; set; }
    }
}
