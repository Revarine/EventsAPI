using Events.Application.CQRS.EventParticipants.Commands.CreateEventParticipant;
using FluentValidation;

namespace Events.Application.Common.Validators.EventParticipants;

public class CreateEventParticipantValidator : AbstractValidator<CreateEventParticipantCommand>
{
    public CreateEventParticipantValidator()
    {
        RuleFor(ep => ep.eventId).NotEmpty().NotNull();
        RuleFor(ep => ep.userId).NotEmpty().NotNull();
    }
}