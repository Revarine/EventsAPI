using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.CreateEvent;

public record CreateEventCommand(string Title, string Description, DateTime EventDate, string Location, string Category, int MaxParticipantsCount, string? ImageFileName, Guid OrganizerId) : IRequest<bool>, IMapFrom<Event>;
