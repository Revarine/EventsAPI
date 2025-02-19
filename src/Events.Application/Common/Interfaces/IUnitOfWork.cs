namespace Events.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task CommitChangesAsync(CancellationToken cancellationToken = default);
}