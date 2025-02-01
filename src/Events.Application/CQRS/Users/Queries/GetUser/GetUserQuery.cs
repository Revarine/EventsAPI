using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<UserDTO>; 