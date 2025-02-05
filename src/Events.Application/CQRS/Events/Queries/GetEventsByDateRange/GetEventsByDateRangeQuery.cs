using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByDateRange;

public record GetEventsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<EventDTO>>;