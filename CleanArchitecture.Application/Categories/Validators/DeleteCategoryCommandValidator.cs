using CleanArchitecture.Application.Categories.Commands;
using FluentValidation;

namespace CleanArchitecture.Application.Categories.Validators;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be positive.");
    }
}
