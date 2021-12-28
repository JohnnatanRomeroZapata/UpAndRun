using FluentValidation;
using UpAndRun.Models;

namespace UpAndRun.Data.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .WithMessage("No empty Value");

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("Must be positive number");
        }
    }
}
