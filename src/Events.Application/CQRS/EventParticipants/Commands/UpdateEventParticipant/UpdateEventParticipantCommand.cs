using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.UpdateEventParticipant;

public record UpdateEventParticipantCommand(Guid userId, Guid eventId) : IRequest<bool>, IMapFrom<EventParticipant>;