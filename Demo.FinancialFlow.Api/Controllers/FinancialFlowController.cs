using Demo.FinancialFlow.Api.Commands;
using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Domain.FinancialFlowAggregate;
using Demo.FinancialFlow.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.FinancialFlow.Api.Controllers
{
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext, IMediator mediator) : ControllerBase
    {
        [HttpGet("health")]
        public async Task<ActionResult<bool>> HealthCheckAsync()
        {
            // TODO : REMOVE AFTER INITIAL TEST PHASE

            //dbContext.FinancialFlows.Add(new Domain.FinancialFlow(100.0f, DateTime.UtcNow, "test123", Domain.FlowType.Past, "testsub123"));

            //await dbContext.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FinancialFlowFile financialFlowFile)
        {
            var file = financialFlowFile.File;

            if (financialFlowFile == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File too large.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            var permittedExtensions = new[] { ".csv" };
            if (!permittedExtensions.Contains(ext))
                return BadRequest("Invalid file type.");

            var processFinancialFlowFileCommand = new ProcessFinancialFlowFileCommand(file);

            await mediator.Send(processFinancialFlowFileCommand);

            return Ok(true);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.FinancialFlowAggregate.FinancialFlow>>> Get([FromQuery] FinancialFlowQuery query)
        {
            var flows = dbContext.FinancialFlows.AsNoTracking().AsQueryable();

            // Filtering
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

            // Pagination
            var totalCount = await flows.CountAsync();
            var items = await flows
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            // Optionally, return totalCount for client-side paging
            return Ok(new { totalCount, items });
        }
    }
}
