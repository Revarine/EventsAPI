using Events.Application.Common.Interfaces;
using MediatR;

namespace Events.Application.CQRS.Events.Notifications.EventDeleted;

public class EventDeletedNotificationHandler : INotificationHandler<EventDeletedNotification>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;

    public EventDeletedNotificationHandler(
        IEmailService emailService,
        IUserRepository userRepository,
        IEventParticipantRepository eventParticipantRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _eventParticipantRepository = eventParticipantRepository;
    }

    public async Task Handle(EventDeletedNotification notification, CancellationToken cancellationToken)
    {
        var participants = await _eventParticipantRepository.GetByEventId(notification.EventId);
        
        foreach (var participant in participants)
        {
            var user = await _userRepository.Get(participant.UserId);
            if (user != null)
            {
                _ = _emailService.SendEventDeletedEmailAsync(notification.EventTitle, user);
            }
        }
    }
} 