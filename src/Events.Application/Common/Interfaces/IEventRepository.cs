using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken = default);
}