using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Common.Repositories.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Name).IsRequired();
        builder.Property(user => user.Surname).IsRequired();
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.Password).IsRequired();
        builder.Property(user => user.DateOfBirth).IsRequired();
        builder.Property(user => user.isAdmin).IsRequired();

        builder.HasMany(user => user.EventParticipants).WithOne(ep => ep.User).HasForeignKey(ep => ep.UserId);
    }
}