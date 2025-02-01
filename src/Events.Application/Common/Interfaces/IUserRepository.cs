using Events.Domain.Entities;

namespace Events.Application.Common.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmail(string email, CancellationToken cancellationToken = default);
}