using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.CQRS.Users.Commands.UpdateUser;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Events.UnitTests.Application.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UpdateUserCommand>> _validatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UpdateUserCommand>>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher>();

        _handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _validatorMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "test@example.com", "John", "Doe", "newpassword", DateTime.UtcNow);
        var existingUser = new User { Id = userId };
        var updatedUser = new User
        {
            Id = userId,
            Email = command.Email,
            Name = command.Name,
            Surname = command.Surname,
            DateOfBirth = command.DateOfBirth
        };

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());
        
        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync(existingUser);
        
        _mapperMock.Setup(x => x.Map<User>(command))
            .Returns(updatedUser);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Update(userId, It.IsAny<User>(), default), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "invalid-email", "John", "Doe", "newpassword", DateTime.UtcNow);
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Invalid email format")
        };

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, default));
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "test@example.com", "John", "Doe", "newpassword", DateTime.UtcNow);

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());
        
        _userRepositoryMock.Setup(x => x.Get(userId, default))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Never);
    }
} 