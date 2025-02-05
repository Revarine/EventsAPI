using Events.Application.CQRS.Users.Commands.UpdateUser;
using FluentValidation;

namespace Events.Application.Common.Validators.Users;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Id).NotEmpty().NotNull();
        RuleFor(u => u.Name).NotEmpty().NotNull().MinimumLength(5).MaximumLength(50);
        RuleFor(u => u.Surname).NotEmpty().NotNull().MinimumLength(5).MaximumLength(50);
        RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(5).MaximumLength(50);
        RuleFor(u => u.Password).NotEmpty().NotNull().MinimumLength(5).MaximumLength(50);
        RuleFor(u => u.DateOfBirth).NotEmpty().NotNull().LessThan(DateTime.Now);
    }
}