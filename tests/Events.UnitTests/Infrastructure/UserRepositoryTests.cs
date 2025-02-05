using Events.Domain.Entities;
using Events.Infrastructure.Common.Persistence;
using Events.Infrastructure.Common.Repositories.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Events.UnitTests.Infrastructure;

public class UserRepositoryTests
{
    private readonly EventDbContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task Create_ShouldAddUserToDatabase()
    {
        var user = new User
        {
            Email = "test@example.com",
            Password = "hashedPassword123",
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        await _userRepository.Create(user);
        await _context.SaveChangesAsync();

        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        savedUser.Should().NotBeNull();
        savedUser!.Name.Should().Be("John");
        savedUser.Surname.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnCorrectUser()
    {
        var user = new User
        {
            Email = "test@example.com",
            Password = "hashedPassword123",
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1)
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _userRepository.GetByEmail("test@example.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
        result.Name.Should().Be("John");
        result.Surname.Should().Be("Doe");
    }
} 