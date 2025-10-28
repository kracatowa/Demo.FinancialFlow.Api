using Demo.FinancialFlow.Api.Commands;
using Demo.FinancialFlow.Api.Controllers.Dto;
using Demo.FinancialFlow.Api.Services.File;
using Demo.FinancialFlow.Api.Services.File.Processors;
using Demo.FinancialFlow.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.FinancialFlow.Api.Controllers
{
    [Route("api/[controller]")]
    public class FinancialFlowFileAuditController(IMediator mediator,
                                                  IValidator<ProcessFile> uploadFileRequestValidator,
                                                  IStorageService storageService,
                                                  IFileProcessorService fileProcessor) : ControllerBase
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
        public async Task<IActionResult> UploadFile([FromBody] ProcessFile processFile)
        {
            var validationResult = uploadFileRequestValidator.Validate(processFile);

            if(!validationResult.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filename = processFile.Subject.Split('/').Last();

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            Guid fileStorageId = Guid.Parse(filenameWithoutExtension);
            var extension = Path.GetExtension(filename).ToLowerInvariant();

            var file = await storageService.Downloadfile(filename);
            var financialFlows = fileProcessor.ProcessFinancialFlow(file, extension, fileStorageId);

            var processFinancialFlowFileCommand = new ProcessBatchFinancialFlowsCommand(financialFlows, fileStorageId);
            await mediator.Send(processFinancialFlowFileCommand);

            return Ok();
        }
    }
}
