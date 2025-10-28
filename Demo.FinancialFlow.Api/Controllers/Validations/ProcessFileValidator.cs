using Demo.FinancialFlow.Api.Controllers.Dto;
using FluentValidation;

namespace Demo.FinancialFlow.Api.Controllers.Validations
{
    public class ProcessFileValidator : AbstractValidator<ProcessFile>
    {
        public ProcessFileValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Cloud event subject is null or empty.")
                .Must(subject =>
                {
                    var filename = subject?.Split('/').Last();
                    return !string.IsNullOrEmpty(Path.GetExtension(filename));
                })
                .WithMessage("File extension is missing.");

            RuleFor(x => x.Subject)
                .Must(subject =>
                {
                    var filename = Path.GetFileNameWithoutExtension(subject?.Split('/').Last());
                    return Guid.TryParse(filename, out _);
                })
                .WithMessage("Invalid storage file id format.");
        }
    }
}
