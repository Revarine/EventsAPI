using Events.API.Models.Requests;
using Events.Application.Common.ResponseDTO;
using Events.Application.CQRS.Events.Commands.CreateEvent;
using Events.Application.CQRS.Events.Commands.DeleteEvent;
using Events.Application.CQRS.Events.Commands.UpdateEvent;
using Events.Application.CQRS.Events.Notifications.EventDeleted;
using Events.Application.CQRS.Events.Notifications.EventUpdated;
using Events.Application.CQRS.Events.Queries.GetAllEvents;
using Events.Application.CQRS.Events.Queries.GetEvent;
using Events.Application.CQRS.Events.Queries.GetEventByTitle;
using Events.Application.CQRS.Events.Queries.GetEventsByCategory;
using Events.Application.CQRS.Events.Queries.GetEventsByDate;
using Events.Application.CQRS.Events.Queries.GetEventsByDateRange;
using Events.Application.CQRS.Events.Queries.GetEventsByLocation;
using Events.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public EventsController(IMediator mediator, IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllEventsQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventDTO>> Get(Guid id)
    {
        var result = await _mediator.Send(new GetEventQuery(id));
        if (result == null) throw new NotFoundException("Event", id);
        return Ok(result);
    }

    [HttpGet("bytitle/{title}")]
    public async Task<ActionResult<EventDTO>> GetEventByTitle(string title)
    {
        var result = await _mediator.Send(new GetEventByTitleQuery(title));
        return Ok(result);
    }

    [HttpGet("bycategory/{category}")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByCategory(string category)
    {
        var result = await _mediator.Send(new GetEventsByCategoryQuery(category));
        return Ok(result);
    }

    [HttpGet("bylocation/{location}")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByLocation(string location)
    {
        var result = await _mediator.Send(new GetEventsByLocationQuery(location));
        return Ok(result);
    }

    [HttpGet("bydate/{date}")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByDate(DateTime date)
    {
        var result = await _mediator.Send(new GetEventsByDateQuery(date));
        return Ok(result);
    }

    [HttpGet("bydaterange")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _mediator.Send(new GetEventsByDateRangeQuery(startDate, endDate));
        return Ok(result);
    }

    [HttpGet("images/{fileName}")]
    public IActionResult GetImage(string fileName)
    {
        var imagePath = Path.Combine(_environment.WebRootPath, "images", fileName);
        if (!System.IO.File.Exists(imagePath))
            throw new NotFoundException("Image", fileName);

        var lastModified = System.IO.File.GetLastWriteTimeUtc(imagePath);
        var etagValue = $"\"{lastModified.Ticks}\"";

        var incomingEtag = Request.Headers.IfNoneMatch.ToString();
        if (incomingEtag == etagValue)
        {
            return StatusCode(StatusCodes.Status304NotModified);
        }

        if (Request.Headers.IfModifiedSince.Count > 0)
        {
            var ifModifiedSince = DateTime.Parse(Request.Headers.IfModifiedSince.ToString()).ToUniversalTime();
            if (lastModified <= ifModifiedSince)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }
        }

        SetCacheHeaders(Response, lastModified, etagValue);

        var imageFileStream = System.IO.File.OpenRead(imagePath);
        return File(imageFileStream, GetContentType(fileName));
    }

    private void SetCacheHeaders(HttpResponse response, DateTime lastModified, string etagValue)
    {
        response.Headers.CacheControl = $"public,max-age={60 * 60 * 24}";
        response.Headers.Expires = DateTime.UtcNow.AddDays(1).ToString("R");
        
        response.Headers.LastModified = lastModified.ToString("R");
        response.Headers.ETag = etagValue;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateEventRequest request)
    {
        string? imageFileName = null;
        try
        {
            if (request.Image != null)
            {
                imageFileName = await SaveImageAsync(request.Image);
            }

            var command = new CreateEventCommand(
                request.Title,
                request.Description,
                request.EventDate,
                request.Location,
                request.Category,
                request.MaxParticipants,
                imageFileName,
                request.OrganizerId);

            await _mediator.Send(command);
            return Ok();
        }
        catch
        {
            if (imageFileName != null)
                await DeleteImageAsync(imageFileName);
            throw;
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateEventRequest request)
    {
        var existingEvent = await _mediator.Send(new GetEventQuery(id));
        if (existingEvent == null) throw new NotFoundException("Event", id);

        string? imageFileName = null;
        try
        {
            if (request.Image != null)
            {
                imageFileName = await SaveImageAsync(request.Image);
            }

            var command = new UpdateEventCommand(
                id,
                request.Title,
                request.Description,
                request.EventDate,
                request.Location,
                request.Category,
                request.MaxParticipants,
                imageFileName ?? existingEvent.ImageFileName,
                request.OrganizerId
            );

            await _mediator.Send(command);

            if (imageFileName != null && existingEvent.ImageFileName != null)
            {
                await DeleteImageAsync(existingEvent.ImageFileName);
            }

            var updatedEvent = await _mediator.Send(new GetEventQuery(id));
            if (updatedEvent != null)
            {
                _ = _mediator.Publish(new EventUpdatedNotification(updatedEvent));
            }

            return Ok();
        }
        catch
        {
            if (imageFileName != null)
                await DeleteImageAsync(imageFileName);
            throw;
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingEvent = await _mediator.Send(new GetEventQuery(id));
        if (existingEvent == null) throw new NotFoundException("Event", id);

        if (existingEvent.ImageFileName != null)
            await DeleteImageAsync(existingEvent.ImageFileName);

        var result = await _mediator.Send(new DeleteEventCommand(id));
        if (!result) throw new NotFoundException("Event", id);

        _ = _mediator.Publish(new EventDeletedNotification(existingEvent.Title, id));

        return Ok();
    }

    private async Task<string> SaveImageAsync(IFormFile image)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(fileStream);

        return uniqueFileName;
    }

    private Task DeleteImageAsync(string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        return Task.CompletedTask;
    }

    private string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream",
        };
    }
}
