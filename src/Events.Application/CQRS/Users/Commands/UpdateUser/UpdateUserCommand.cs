using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string Username, string Email, string FirstName, string LastName) : IRequest<bool>, IMapFrom<User>;