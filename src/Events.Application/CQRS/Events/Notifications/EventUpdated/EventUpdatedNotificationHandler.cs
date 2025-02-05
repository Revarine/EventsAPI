using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Notifications.EventUpdated;

public class EventUpdatedNotificationHandler : INotificationHandler<EventUpdatedNotification>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;

    public EventUpdatedNotificationHandler(
        IEmailService emailService,
        IUserRepository userRepository,
        IEventParticipantRepository eventParticipantRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _eventParticipantRepository = eventParticipantRepository;
    }

    public async Task Handle(EventUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var participants = await _eventParticipantRepository.GetByEventId(notification.Event.Id, cancellationToken);
        
        foreach (var participant in participants)
        {
            var user = await _userRepository.Get(participant.UserId, cancellationToken);
            if (user != null)
            {
                _ = _emailService.SendEventUpdatedEmailAsync(notification.Event, user);
            }
        }
    }
} 