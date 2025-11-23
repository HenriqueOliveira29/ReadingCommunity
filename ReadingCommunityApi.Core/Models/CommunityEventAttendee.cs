namespace ReadingCommunityApi.Core.Models;

public class CommunityEventAttendee
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public CommunityEvent Event { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; }
    
    public AttendeeStatus Status { get; private set; }
    public DateTime RespondedAt { get; private set; }
    
    private CommunityEventAttendee() { } // EF Core
    
    public CommunityEventAttendee(int eventId, int userId, AttendeeStatus status)
    {
        EventId = eventId;
        UserId = userId;
        Status = status;
        RespondedAt = DateTime.UtcNow;
    }
    
    public void UpdateStatus(AttendeeStatus newStatus)
    {
        Status = newStatus;
        RespondedAt = DateTime.UtcNow;
    }
}

public enum AttendeeStatus
{
    Going = 0,
    Maybe = 1,
    NotGoing = 2
}