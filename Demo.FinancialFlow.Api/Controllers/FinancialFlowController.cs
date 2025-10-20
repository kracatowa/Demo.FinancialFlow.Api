using Demo.FinancialFlow.Api.Commands;
using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.FinancialFlow.Api.Controllers
{
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext, IMediator mediator): ControllerBase
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
        public async Task<IActionResult> UploadFile([FromForm] FinancialFlowFileDto financialFlowFile)
        {
            var file = financialFlowFile.File;

            if (financialFlowFile == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File too large.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            var permittedExtensions = new[] { ".csv"};
            if (!permittedExtensions.Contains(ext))
                return BadRequest("Invalid file type.");

            var processFinancialFlowFileCommand = new ProcessFinancialFlowFileCommand(file);

            await mediator.Send(processFinancialFlowFileCommand);

            return Ok(true);
        }
    }
}
