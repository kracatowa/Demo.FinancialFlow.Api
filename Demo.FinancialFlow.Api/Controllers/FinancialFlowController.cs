using Demo.FinancialFlow.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Demo.FinancialFlow.Api.Controllers
{
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext): ControllerBase
    {
        [HttpGet("health")]
        public ActionResult<bool> HealthCheck()
        {
            return Ok(dbContext.Database.CanConnect());
        }
    }
}
