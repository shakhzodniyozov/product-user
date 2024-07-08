using FluentValidation;

namespace Application;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be null.");
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}
