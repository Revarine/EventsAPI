using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.DeleteEvent;

public record DeleteEventCommand(Guid Id) : IRequest<bool>, IMapFrom<Event>; 