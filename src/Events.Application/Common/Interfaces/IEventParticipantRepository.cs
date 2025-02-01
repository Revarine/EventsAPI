using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IEventParticipantRepository : IBaseRepository<EventParticipant>
{
    Task<IEnumerable<EventParticipant>> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EventParticipant>> GetByEventId(Guid eventId, CancellationToken cancellationToken = default);
}
