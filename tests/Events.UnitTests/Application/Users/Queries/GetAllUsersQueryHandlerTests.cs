using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.Users.Queries.GetAllUsers;
using Events.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Events.UnitTests.Application.Users.Queries;

public class GetAllUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllUsersQueryHandler _handler;

    public GetAllUsersQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedUserDTOs()
    {
        // Arrange
        var query = new GetAllUsersQuery(1, 10);
        var users = new List<User>
        {
            new User { Email = "user1@example.com", Name = "User1" },
            new User { Email = "user2@example.com", Name = "User2" }
        };

        _userRepositoryMock.Setup(x => x.GetAll(query.page, query.pageSize, default))
            .ReturnsAsync(users);
        
        _mapperMock.Setup(x => x.Map<UserDTO>(It.Is<User>(u => u.Email == "user1@example.com")))
            .Returns(new UserDTO { Email = "user1@example.com", Name = "User1" });
        _mapperMock.Setup(x => x.Map<UserDTO>(It.Is<User>(u => u.Email == "user2@example.com")))
            .Returns(new UserDTO { Email = "user2@example.com", Name = "User2" });

        // Act
        var result = (await _handler.Handle(query, default)).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Email.Should().Be("user1@example.com");
        result[1].Email.Should().Be("user2@example.com");
        _userRepositoryMock.Verify(x => x.GetAll(query.page, query.pageSize, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(It.Is<User>(u => u.Email == "user1@example.com")), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(It.Is<User>(u => u.Email == "user2@example.com")), Times.Once);
    }

    [Fact]
    public async Task Handle_NoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllUsersQuery(1, 10);
        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.GetAll(query.page, query.pageSize, default))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _userRepositoryMock.Verify(x => x.GetAll(query.page, query.pageSize, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(It.IsAny<User>()), Times.Never);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(0, 0)]
    public async Task Handle_InvalidPagination_ShouldReturnEmptyList(int page, int pageSize)
    {
        // Arrange
        var query = new GetAllUsersQuery(page, pageSize);
        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.GetAll(page, pageSize, default))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _userRepositoryMock.Verify(x => x.GetAll(page, pageSize, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(It.IsAny<User>()), Times.Never);
    }
} 