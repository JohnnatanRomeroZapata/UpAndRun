using FluentValidation;
using UpAndRun.Models;

namespace UpAndRun.Data.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.CategoryName)
                .NotNull()
                .WithMessage("No empty Value")
                .Length(5, 1000)
                .WithMessage("Must be between 5 and 1000 chars");

            RuleFor(c => c.DisplayOrder)
                    .NotNull()
                    .WithMessage("Cannot be empty")
                    .GreaterThan(0)
                    .WithMessage("Must be greater than 0");
        }
    }
}
