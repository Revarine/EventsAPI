using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByDate;

public record GetEventsByDateQuery(DateTime Date) : IRequest<IEnumerable<EventDTO>>;