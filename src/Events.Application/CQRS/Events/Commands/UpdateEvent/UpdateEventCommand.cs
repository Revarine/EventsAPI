using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.UpdateEvent;

public record UpdateEventCommand(Guid Id, string Title, string Description, DateTime EventDate, string Location, string Category, int MaxParticipantsCount, string ImageFileName, Guid OrganizerId) : IRequest<bool>, IMapFrom<Event>; 