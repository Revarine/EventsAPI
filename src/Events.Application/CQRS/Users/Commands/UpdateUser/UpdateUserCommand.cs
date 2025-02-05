using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string Email, string Password, string Name, string Surname, DateTime DateOfBirth) : IRequest<bool>, IMapFrom<User>;