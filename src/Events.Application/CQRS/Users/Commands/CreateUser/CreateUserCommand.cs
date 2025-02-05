using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password, string Name, string Surname, DateTime DateOfBirth) : IRequest<bool>, IMapFrom<User>;
