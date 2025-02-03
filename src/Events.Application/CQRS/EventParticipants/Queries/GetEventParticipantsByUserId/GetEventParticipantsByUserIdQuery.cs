using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventParticipantsByUserId;

public record GetEventParticipantsByUserIdQuery(Guid userId) : IRequest<IEnumerable<EventParticipantDTO>>;