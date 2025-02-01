using System.Diagnostics.CodeAnalysis;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Common.Repositories.Events;

public class EventRepository : IEventRepository
{
    private readonly EventDbContext _eventDbContext;
    public EventRepository(EventDbContext eventDbContext)
    {
        _eventDbContext = eventDbContext;
    }
    public async Task Create(Event entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Events.AddAsync(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Events.Where(ev => ev.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Event> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.FirstAsync(ev => ev.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.ToListAsync(cancellationToken);
    }

    public async Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.FirstAsync(ev => ev.Title == title, cancellationToken);
    }

    public async Task Update(Guid id, Event entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Events.Where(ev => ev.Id == id).ExecuteUpdateAsync
        (
            p => p
                .SetProperty(e => e.Title, entity.Title)
                .SetProperty(e => e.Description, entity.Description)
                .SetProperty(e => e.EventDate, entity.EventDate)
                .SetProperty(e => e.Location, entity.Location)
                .SetProperty(e => e.Category, entity.Category)
                .SetProperty(e => e.MaxParticipantsCount, entity.MaxParticipantsCount)
                .SetProperty(e => e.ImageFileName, entity.ImageFileName),
                cancellationToken
        );
    }
}