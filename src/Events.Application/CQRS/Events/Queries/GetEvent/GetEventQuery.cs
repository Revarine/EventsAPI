using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEvent;

public record GetEventQuery(Guid Id) : IRequest<EventDTO>; 