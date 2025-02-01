namespace Events.Application.Common.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T entity, CancellationToken cancellationToken = default);
    Task<T> Get(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
    Task Update(Guid id, T entity, CancellationToken cancellationToken = default);
    Task Delete(Guid id, CancellationToken cancellationToken = default);
}