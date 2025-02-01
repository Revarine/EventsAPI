using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.UpdateEvent;

public record UpdateEventCommand(Guid Id, string Title, string Description, DateTime StartDate, DateTime EndDate, string Location, int MaxParticipants) : IRequest<bool>, IMapFrom<Event>; 