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
        return await _eventDbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAll(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.FirstOrDefaultAsync(ev => ev.Title == title, cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByCategory(string category, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.Where(ev => ev.Category == category).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByDate(DateTime date, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.Where(ev => ev.EventDate == date).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByDateRange(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.Where(ev => ev.EventDate >= startDate && ev.EventDate <= endDate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByLocation(string location, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Events.Where(ev => ev.Location == location).ToListAsync(cancellationToken);
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