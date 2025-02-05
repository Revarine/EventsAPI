using MediatR;

namespace Events.Application.CQRS.Events.Notifications.EventDeleted;

public record EventDeletedNotification(string EventTitle, Guid EventId) : INotification; 