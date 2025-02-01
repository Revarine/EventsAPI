namespace Events.Domain.Entities;

public class EventParticipant : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public Guid EventId { get; set; }

    public virtual Event Event { get; set; } = null!;


    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}   