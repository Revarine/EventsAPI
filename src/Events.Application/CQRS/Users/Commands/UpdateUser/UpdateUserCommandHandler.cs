using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.Id);
        if (user == null) return false;

        await _userRepository.Update(request.Id, _mapper.Map<User>(request), cancellationToken);
        return true;
    }
} 