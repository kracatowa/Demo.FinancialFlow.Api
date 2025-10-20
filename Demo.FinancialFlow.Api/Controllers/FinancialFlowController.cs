using Demo.FinancialFlow.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Demo.FinancialFlow.Api.Controllers
{
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext): ControllerBase
    {
        [HttpGet("health")]
        public async Task<ActionResult<bool>> HealthCheckAsync()
        {
            // TODO : REMOVE AFTER INITIAL TEST PHASE

            //dbContext.FinancialFlows.Add(new Domain.FinancialFlow(100.0f, DateTime.UtcNow, "test123", Domain.FlowType.Past, "testsub123"));

            //await dbContext.SaveChangesAsync();

            return Ok(true);
        }
    }
}
