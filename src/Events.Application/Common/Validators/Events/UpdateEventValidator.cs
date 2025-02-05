using Events.Application.CQRS.Events.Commands.UpdateEvent;
using FluentValidation;

namespace Events.Application.Common.Validators.Events;

public class UpdateEventValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventValidator()
    {
        RuleFor(ev => ev.Title).NotEmpty().NotNull().MinimumLength(5).MaximumLength(50);
        RuleFor(ev => ev.Description).NotEmpty().NotNull().MinimumLength(5).MaximumLength(1000);
        RuleFor(ev => ev.EventDate).NotEmpty().NotNull().LessThan(ev => DateTime.Now.AddDays(30));
        RuleFor(ev => ev.Location).NotEmpty().NotNull().MinimumLength(5).MaximumLength(100);
        RuleFor(ev => ev.Category).NotEmpty().NotNull().MinimumLength(5).MaximumLength(50);
        RuleFor(ev => ev.MaxParticipantsCount).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(ev => ev.OrganizerId).NotEmpty().NotNull();
    }
}