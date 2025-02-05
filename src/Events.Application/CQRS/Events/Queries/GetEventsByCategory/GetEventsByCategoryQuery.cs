using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByCategory;

public record GetEventsByCategoryQuery(string Category) : IRequest<IEnumerable<EventDTO>>;