using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.Users.Queries.GetUser;
using Events.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Events.UnitTests.Application.Users.Queries;

public class GetUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingUser_ShouldReturnUserDTO()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery(userId);
        var user = new User 
        { 
            Id = userId,
            Email = "test@example.com",
            Name = "John",
            Surname = "Doe",
            DateOfBirth = DateTime.UtcNow,
            Password = "hashedpassword",
            isAdmin = false
        };
        var userDto = new UserDTO
        {
            Email = "test@example.com",
            Name = "John",
            Surname = "Doe",
            DateOfBirth = user.DateOfBirth,
            Password = "hashedpassword",
            isAdmin = false
        };

        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync(user);
        
        _mapperMock.Setup(x => x.Map<UserDTO>(user))
            .Returns(userDto);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.Name.Should().Be("John");
        result.Surname.Should().Be("Doe");
        result.DateOfBirth.Should().Be(user.DateOfBirth);
        result.Password.Should().Be("hashedpassword");
        result.isAdmin.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Get(userId, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingUser_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery(userId);

        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync((User?)null);
        
        _mapperMock.Setup(x => x.Map<UserDTO?>(null))
            .Returns((UserDTO?)null);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().BeNull();
        _userRepositoryMock.Verify(x => x.Get(userId, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO?>(null), Times.Once);
    }
}