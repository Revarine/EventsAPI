using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Notifications.EventUpdated;

public record EventUpdatedNotification(EventDTO Event) : INotification; 