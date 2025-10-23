using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class StartUploadFinancialFlowFileCommand(string userId, IFormFile file) : IRequest<bool>
    {
        public IFormFile File { get; } = file;
        public string UserId { get; } = userId;
    }
}
