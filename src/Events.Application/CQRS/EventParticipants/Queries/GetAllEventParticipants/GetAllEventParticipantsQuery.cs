using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetAllEventParticipants;

public record class GetAllEventParticipantsQuery(int page, int pageSize) : IRequest<IEnumerable<EventParticipantDTO>>;