using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventParticipant;

public record GetEventParticipantQuery(Guid Id) : IRequest<EventParticipantDTO>;