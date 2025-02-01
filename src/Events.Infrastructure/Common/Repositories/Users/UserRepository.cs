using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Common.Repositories.Users;

public class UserRepository : IUserRepository
{
    private EventDbContext _eventDbContext;
    public UserRepository(EventDbContext eventDbContext)
    {
        _eventDbContext = eventDbContext;
    }
    public async Task Create(User entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Users.AddAsync(entity);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<User> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Users.FirstAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await _eventDbContext.Users.FirstAsync(u => u.Email == email, cancellationToken);
    }

    public async Task Update(Guid id, User entity, CancellationToken cancellationToken = default)
    {
        await _eventDbContext.Users.Where(u => u.Id == id).ExecuteUpdateAsync
        (
            p => p
                .SetProperty(e => e.Name, entity.Name)
                .SetProperty(e => e.Surname, entity.Surname)
                .SetProperty(e => e.Email, entity.Email)
                .SetProperty(e => e.Password, entity.Password)
                .SetProperty(e => e.DateOfBirth, entity.DateOfBirth),
                cancellationToken
        );
    }
}