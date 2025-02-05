using Events.Application.CQRS.EventParticipants.Commands.UpdateEventParticipant;
using FluentValidation;

namespace Events.Application.Common.Validators.EventParticipants;

public class UpdateEventParticipantValidator : AbstractValidator<UpdateEventParticipantCommand>
{
    public UpdateEventParticipantValidator()
    {
        RuleFor(ep => ep.eventId).NotEmpty().NotNull();
        RuleFor(ep => ep.userId).NotEmpty().NotNull();
    }
}