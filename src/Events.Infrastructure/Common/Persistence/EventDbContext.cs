using System.Reflection;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Common.Persistence;

public class EventDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<EventParticipant> EventParticipants { get; set; } = null!;

    public EventDbContext(DbContextOptions options) : base(options)
    {   
    }

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}