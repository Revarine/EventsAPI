using Events.Application.Common.Interfaces;
using Events.Application.CQRS.Users.Commands.DeleteUser;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using AutoMapper;

namespace Events.UnitTests.Application.Users.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingUser_ShouldDeleteUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);
        var existingUser = new User { Id = userId };

        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Delete(userId, default), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingUser_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);

        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Delete(userId, default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Never);
    }
} 