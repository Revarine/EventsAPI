using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IValidator<UpdateUserCommand> validator, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        var user = await _userRepository.Get(request.Id);
        if (user == null) throw new NotFoundException("User", request.Id);

        var updatedUser = _mapper.Map<User>(request);
        
        if (!string.IsNullOrEmpty(request.Password))
        {
            updatedUser.Password = _passwordHasher.HashPassword(request.Password);
        }
        else 
        {
            updatedUser.Password = user.Password;
        }

        await _userRepository.Update(request.Id, updatedUser, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
} 