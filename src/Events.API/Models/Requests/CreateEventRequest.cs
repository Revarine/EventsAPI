using Events.Application.CQRS.Events.Commands.CreateEvent;

namespace Events.API.Models.Requests;

public class CreateEventRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxParticipants { get; set; }
    public IFormFile? Image { get; set; }
    public Guid OrganizerId { get; set; }
} 