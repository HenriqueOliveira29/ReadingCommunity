namespace ReadingCommunityApi.Core.Models;
public class CommunityEvent
{
    public int Id { get; private set; }
    public int CommunityId { get; private set; }
    public Community Community { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime EventDate { get; private set; }
    public string Location { get; private set; }

    public int? BookId { get; private set; }
    public Book? Book { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<CommunityEventAttendee> Attendees { get; private set; } = new List<CommunityEventAttendee>();

    private CommunityEvent() { } // EF Core

    public CommunityEvent(int communityId, string title, string description,
                          DateTime eventDate, string location, int? bookId = null)
    {
        CommunityId = communityId;
        Title = title;
        Description = description;
        EventDate = eventDate;
        Location = location;
        BookId = bookId;
        CreatedAt = DateTime.UtcNow;
    }
}