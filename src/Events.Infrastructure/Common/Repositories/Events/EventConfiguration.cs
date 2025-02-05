using System.Security.Cryptography.X509Certificates;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Common.Repositories.Events;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(ev => ev.Id);

        builder.Property(ev => ev.Title).IsRequired();
        builder.Property(ev => ev.Description).IsRequired();
        builder.Property(ev => ev.EventDate).IsRequired();
        builder.Property(ev => ev.Location).IsRequired();
        builder.Property(ev => ev.Category).IsRequired();
        builder.Property(ev => ev.MaxParticipantsCount).IsRequired();
        builder.Property(ev => ev.ImageFileName);

        builder.HasOne(ev => ev.User)
        .WithMany(user => user.Events)
        .HasForeignKey(ev => ev.OrganizerId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ev => ev.EventParticipants)
        .WithOne(ep => ep.Event)
        .HasForeignKey(ep => ep.EventId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}