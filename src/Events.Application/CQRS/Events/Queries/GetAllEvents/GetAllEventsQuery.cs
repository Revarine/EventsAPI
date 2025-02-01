using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetAllEvents;

public record GetAllEventsQuery() : IRequest<IEnumerable<EventDTO>>; 