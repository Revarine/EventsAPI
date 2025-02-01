using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.DeleteEventParticipant;

public record DeleteEventParticipantCommand(Guid id) : IRequest<bool>, IMapFrom<EventParticipant>;