using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.Id);
        if (user == null) return false;

        await _userRepository.Delete(request.Id, cancellationToken);
        return true;
    }
} 