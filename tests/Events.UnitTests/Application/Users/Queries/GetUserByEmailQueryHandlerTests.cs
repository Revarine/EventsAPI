using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.Users.Queries.GetUserByEmail;
using Events.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Events.UnitTests.Application.Users.Queries;

public class GetUserByEmailQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByEmailQueryHandler _handler;

    public GetUserByEmailQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserByEmailQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingEmail_ShouldReturnUserDTO()
    {
        // Arrange
        var email = "test@example.com";
        var query = new GetUserByEmailQuery(email);
        var user = new User
        {
            Email = email,
            Name = "John",
            Surname = "Doe"
        };
        var userDto = new UserDTO
        {
            Email = email,
            Name = "John",
            Surname = "Doe"
        };

        _userRepositoryMock.Setup(x => x.GetByEmail(email, default))
            .ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDTO>(user))
            .Returns(userDto);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        result.Name.Should().Be("John");
        result.Surname.Should().Be("Doe");
        _userRepositoryMock.Verify(x => x.GetByEmail(email, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingEmail_ShouldReturnNull()
    {
        // Arrange
        var email = "nonexisting@example.com";
        var query = new GetUserByEmailQuery(email);

        _userRepositoryMock.Setup(x => x.GetByEmail(email, default))
            .ReturnsAsync((User?)null);
        
        _mapperMock.Setup(x => x.Map<UserDTO>(null))
            .Returns((UserDTO)null);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().BeNull();
        _userRepositoryMock.Verify(x => x.GetByEmail(email, default), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDTO>(null), Times.Once);
    }
} 