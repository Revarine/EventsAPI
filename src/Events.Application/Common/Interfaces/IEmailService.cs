using Events.Application.Common.ResponseDTO;
using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEventUpdatedEmailAsync(EventDTO @event, User participant);
    Task SendEventDeletedEmailAsync(string eventTitle, User participant);
} 