using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmail(request.Email);
        if (existingUser != null) return false;

        var user = _mapper.Map<User>(request);
        await _userRepository.Create(user, cancellationToken);
        return true;
    }
} 