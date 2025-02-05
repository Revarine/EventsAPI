using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetAllEvents;

public record GetAllEventsQuery(int page, int pageSize) : IRequest<IEnumerable<EventDTO>>; 