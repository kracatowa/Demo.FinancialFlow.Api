using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessFinancialFlowFileCommand(IFormFile file) : IRequest<Unit>
    {
        public IFormFile File { get; } = file;
    }
}
