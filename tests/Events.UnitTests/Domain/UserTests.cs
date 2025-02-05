using Events.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Events.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveDefaultValues_WhenCreated()
    {
        var user = new User();

        user.Id.Should().NotBeEmpty();
        user.Events.Should().NotBeNull();
        user.EventParticipants.Should().NotBeNull();
        user.DateOfBirth.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.isAdmin.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldHaveCorrectValues_WhenPropertiesAreSet()
    {
        var user = new User
        {
            Email = "test@example.com",
            Password = "hashedPassword123",
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        user.Email.Should().Be("test@example.com");
        user.Password.Should().Be("hashedPassword123");
        user.Name.Should().Be("John");
        user.Surname.Should().Be("Doe");
        user.DateOfBirth.Should().Be(new DateTime(1990, 1, 1));
    }
} 