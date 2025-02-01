using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<bool>, IMapFrom<User>; 