using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.CQRS.Users.Commands.CreateUser;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Events.UnitTests.Application.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IValidator<CreateUserCommand>> _validatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _validatorMock = new Mock<IValidator<CreateUserCommand>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _validatorMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand("test@example.com", "password", "John", "Doe", DateTime.UtcNow);
        var user = new User
        {
            Email = command.Email,
            Name = command.Name,
            Surname = command.Surname,
            DateOfBirth = command.DateOfBirth
        };

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());
        
        _userRepositoryMock.Setup(x => x.GetByEmail(command.Email, default))
            .ReturnsAsync((User?)null);
        
        _mapperMock.Setup(x => x.Map<User>(command))
            .Returns(user);
        
        _passwordHasherMock.Setup(x => x.HashPassword(command.Password))
            .Returns("hashedPassword");

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>(), default), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateUserCommand("invalid-email", "pwd", "John", "Doe", DateTime.UtcNow);
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Invalid email format")
        };

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, default));
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_ShouldThrowAlreadyExistsException()
    {
        // Arrange
        var command = new CreateUserCommand("existing@example.com", "password", "John", "Doe", DateTime.UtcNow);
        var existingUser = new User { Email = command.Email };

        _validatorMock.Setup(x => x.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());
        
        _userRepositoryMock.Setup(x => x.GetByEmail(command.Email, default))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, default));
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(default), Times.Never);
    }
} 