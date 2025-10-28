using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.FinancialFlowAggregate.FinancialFlow>>> Get([FromQuery] FinancialFlowQuery query)
        {
            var flows = dbContext.FinancialFlows.AsNoTracking().AsQueryable();

            if (query.MinAmount.HasValue)
                flows = flows.Where(f => f.Amount >= query.MinAmount.Value);
            if (query.MaxAmount.HasValue)
                flows = flows.Where(f => f.Amount <= query.MaxAmount.Value);
            if (query.FromDate.HasValue)
                flows = flows.Where(f => f.TransactionDate >= query.FromDate.Value);
            if (query.ToDate.HasValue)
                flows = flows.Where(f => f.TransactionDate <= query.ToDate.Value);
            if (!string.IsNullOrEmpty(query.Description))
                flows = flows.Where(f => f.Description.Contains(query.Description));
            if (query.FlowType.HasValue)
                flows = flows.Where(f => f.FlowType == query.FlowType.Value);
            if (!string.IsNullOrEmpty(query.Subsidiairy))
                flows = flows.Where(f => f.Subsidiairy == query.Subsidiairy);

            var totalCount = await flows.CountAsync();
            var items = await flows
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return Ok(new { totalCount, items });
        }
    }
}
