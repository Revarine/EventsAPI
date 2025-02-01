using System.Reflection;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Common.Persistence;

public class EventDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users = null!;
    public DbSet<Event> Events = null!;
    public DbSet<EventParticipant> EventParticipants = null!;

    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {   
    }

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}