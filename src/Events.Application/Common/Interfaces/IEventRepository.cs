using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsByDate(DateTime date, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsByDateRange(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsByLocation(string location, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsByCategory(string category, CancellationToken cancellationToken = default);
}