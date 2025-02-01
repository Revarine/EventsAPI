using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetAllEventParticipants;

public record class GetAllEventParticipantsQuery() : IRequest<IEnumerable<EventParticipantDTO>>;