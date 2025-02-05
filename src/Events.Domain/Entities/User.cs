namespace Events.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
    public bool isAdmin = false;

    public virtual ICollection<Event> Events { get; set; } = [];
    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = [];
}
