using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.CreateEvent;

public record CreateEventCommand(string Title, string Description, DateTime StartDate, DateTime EndDate, string Location, int MaxParticipants, Guid OrganizerId) : IRequest<bool>, IMapFrom<Event>;
