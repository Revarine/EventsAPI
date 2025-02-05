using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Users.Queries.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IRequest<UserDTO>;