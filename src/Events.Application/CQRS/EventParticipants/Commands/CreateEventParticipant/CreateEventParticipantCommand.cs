using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.CreateEventParticipant;

public record CreateEventParticipantCommand(Guid userId, Guid eventId) : IRequest<bool>, IMapFrom<EventParticipant>;