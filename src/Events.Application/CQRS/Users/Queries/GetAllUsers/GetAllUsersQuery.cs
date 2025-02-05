using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(int page, int pageSize) : IRequest<IEnumerable<UserDTO>>; 