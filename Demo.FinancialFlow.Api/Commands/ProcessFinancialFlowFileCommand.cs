using MediatR;

namespace Demo.FinancialFlow.Api.Commands
{
    public class ProcessFinancialFlowFileCommand(Guid fileStorageId, string extension, string filename) : IRequest<Unit>
    {
        public Guid FileStorageId { get; } = fileStorageId;
        public string Extension { get; } = extension;
        public string Filename { get; } = filename;
    }
}
