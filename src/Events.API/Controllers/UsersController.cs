using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.Users.Commands.CreateUser;
using Events.Application.CQRS.Users.Commands.DeleteUser;
using Events.Application.CQRS.Users.Commands.UpdateUser;
using Events.Application.CQRS.Users.Queries.GetAllUsers;
using Events.Application.CQRS.Users.Queries.GetUser;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> Get(Guid id)
    {
        var result = await _mediator.Send(new GetUserQuery(id));
        if (result == null) throw new NotFoundException("User", id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id) throw new ValidationException($"Route id ({id}) does not match command id ({command.Id})");
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        if (!result) throw new NotFoundException("User", id);
        return Ok();
    }
}
