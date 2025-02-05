using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Common.Repositories.EventParticipants;

public class EventParticipantRepository : IEventParticipantRepository
{
    private readonly EventDbContext _eventDbContext;

    public EventParticipantRepository(EventDbContext eventDbContext)
    {
        _eventDbContext = eventDbContext;
    }
    public async Task Create(EventParticipant entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.EventParticipants.AddAsync(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.EventParticipants.Where(ep => ep.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<EventParticipant> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.EventParticipants.FirstOrDefaultAsync(ep => ep.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<EventParticipant>> GetAll(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.EventParticipants.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EventParticipant>> GetByEventId(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.EventParticipants.Where(ep => ep.EventId == eventId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EventParticipant>> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.EventParticipants.Where(ep => ep.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task Update(Guid id, EventParticipant entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.EventParticipants.Where(ep => ep.Id == id).ExecuteUpdateAsync
        (
            p => p
                .SetProperty(e => e.EventId, entity.Id)
                .SetProperty(e => e.UserId, entity.UserId)
                .SetProperty(e => e.RegistrationDate, entity.RegistrationDate),
                cancellationToken
        );
    }
}