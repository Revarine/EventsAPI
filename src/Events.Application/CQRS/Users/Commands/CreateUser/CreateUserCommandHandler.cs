using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IValidator<CreateUserCommand> validator, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
        if (existingUser != null) throw new AlreadyExistsException("User with email", request.Email);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordHasher.HashPassword(request.Password);
        
        await _userRepository.Create(user, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
} 