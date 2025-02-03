using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventPaticipantsByEventId;

public record GetEventParticipantsByEventIdQuery(Guid eventId) : IRequest<IEnumerable<EventParticipantDTO>>;