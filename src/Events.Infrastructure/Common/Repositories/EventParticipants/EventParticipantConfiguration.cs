using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Common.Repositories.EventParticipants;

public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
{
    public void Configure(EntityTypeBuilder<EventParticipant> builder)
    {
        builder.HasKey(ep => ep.Id);

        builder.Property(ep => ep.RegistrationDate);
        
        builder.HasOne(ep => ep.User)
        .WithMany(user => user.EventParticipants)
        .HasForeignKey(ep => ep.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ep => ep.Event)
        .WithMany(ev => ev.EventParticipants)
        .HasForeignKey(ep => ep.EventId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}