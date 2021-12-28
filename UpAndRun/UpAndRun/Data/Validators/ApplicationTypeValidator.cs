using FluentValidation;
using UpAndRun.Models;

namespace UpAndRun.Data.Validators
{
    public class ApplicationTypeValidator : AbstractValidator<ApplicationType>
    {
        public ApplicationTypeValidator()
        {
            RuleFor(a => a.ApplicationName)
                .NotNull()
                .WithMessage("Can not be empty")
                .Length(5, 1000)
                .WithMessage("Must be between 5 and 1000 chars");
        }
    }
}
