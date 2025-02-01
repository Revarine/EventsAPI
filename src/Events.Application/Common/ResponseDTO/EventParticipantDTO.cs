using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;

namespace Events.Application.Common.ResponseDTO;

public class EventParticipantDTO : IMapFrom<EventParticipant>
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public DateTime RegistrationDate { get; set; }
}