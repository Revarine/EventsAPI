using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.EventParticipants.Commands.CreateEventParticipant;
using Events.Application.CQRS.EventParticipants.Commands.DeleteEventParticipant;
using Events.Application.CQRS.EventParticipants.Commands.UpdateEventParticipant;
using Events.Application.CQRS.EventParticipants.Queries.GetAllEventParticipants;
using Events.Application.CQRS.EventParticipants.Queries.GetEventParticipant;
using Events.Application.CQRS.EventParticipants.Queries.GetEventParticipantsByUserId;
using Events.Application.CQRS.EventParticipants.Queries.GetEventPaticipantsByEventId;
using Events.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventParticipantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventParticipantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventParticipantDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllEventParticipantsQuery(page, pageSize));
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventParticipantDTO>> Get(Guid id)
    {
        var result = await _mediator.Send(new GetEventParticipantQuery(id));
        if (result == null) throw new NotFoundException("EventParticipant", id);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("event/{eventId:guid}")]
    public async Task<ActionResult<IEnumerable<EventParticipantDTO>>> GetByEventId(Guid eventId)
    {
        var result = await _mediator.Send(new GetEventParticipantsByEventIdQuery(eventId));
        return Ok(result);
    }

    [Authorize]
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<EventParticipantDTO>>> GetByUserId(Guid userId)
    {
        var result = await _mediator.Send(new GetEventParticipantsByUserIdQuery(userId));
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventParticipantCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEventParticipantCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteEventParticipantCommand(id));
        if (!result) throw new NotFoundException("EventParticipant", id);
        return Ok();
    }
}
