using Events.Application.Common.AutoMapper;
using Events.Domain.Entities;

namespace Events.Application.Common.ResponseDTO;

public class EventDTO : IMapFrom<Event>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxParticipantsCount { get; set; }
    public string? ImageFileName { get; set; }
}