using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByLocation;

public record GetEventsByLocationQuery(string Location) : IRequest<IEnumerable<EventDTO>>;