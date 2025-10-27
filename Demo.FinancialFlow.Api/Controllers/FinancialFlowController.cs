using Demo.FinancialFlow.Api.Commands;
using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.FinancialFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialFlowController(FinancialFlowContext dbContext, IMediator mediator, IValidator<UploadFileRequest> uploadFileRequestValidator) : ControllerBase
    {
        [HttpPost("upload/start")]
        public async Task<IActionResult> StartUploadFile([FromForm] StartFinancialFlowFile startFinancialFlowFile)
        {
            var file = startFinancialFlowFile.File;

            if (startFinancialFlowFile == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File too large.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            var permittedExtensions = new[] { ".csv" };
            if (!permittedExtensions.Contains(ext))
                return BadRequest("Invalid file type.");

            var startFinancialFlowFileCommand = new StartUploadFinancialFlowFileCommand(startFinancialFlowFile.UserId, file);

            var result = await mediator.Send(startFinancialFlowFileCommand);

            if (result is false)
            {
                return BadRequest("File upload failed.");
            }

            return Ok(true);
        }

        [HttpPost("upload/process")]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            uploadFileRequestValidator.ValidateAndThrow(request);

            var filename = request.Subject.Split('/').Last();

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename).ToLowerInvariant();

            var processFinancialFlowFileCommand = new ProcessFinancialFlowFileCommand(Guid.Parse(filenameWithoutExtension), extension, filename);
            await mediator.Send(processFinancialFlowFileCommand);

            return Ok(true);
        }

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
