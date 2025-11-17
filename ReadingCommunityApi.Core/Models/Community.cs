namespace ReadingCommunityApi.Core.Models;
public class Community
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public CommunityType Type { get; private set; }
    public bool IsPrivate { get; private set; }

    public int CreatorId { get; private set; }
    public User Creator { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public ICollection<Category> FocusCategories { get; private set; } = new List<Category>();

    public ICollection<CommunityMember> Members { get; private set; } = new List<CommunityMember>();
    public ICollection<CommunityPost> Posts { get; private set; } = new List<CommunityPost>();
    public ICollection<CommunityEvent> Events { get; private set; } = new List<CommunityEvent>();

    // Each community can have its own chat
    public int? ConversationId { get; private set; }
    public Conversation Conversation { get; private set; }

    private Community() { } // EF Core

    public Community(string name, string description, int creatorId,
                     CommunityType type, bool isPrivate = false, string imageUrl = null)
    {
        Name = name;
        Description = description;
        CreatorId = creatorId;
        Type = type;
        IsPrivate = isPrivate;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;

        // Add creator as admin
        Members.Add(new CommunityMember(Id, creatorId, CommunityRole.Admin));
    }

    public void Update(string name, string description, string imageUrl, bool isPrivate)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPrivate = isPrivate;
    }

    public void AddMember(int userId, CommunityRole role = CommunityRole.Member)
    {
        if (!Members.Any(m => m.UserId == userId && m.LeftAt == null))
        {
            Members.Add(new CommunityMember(Id, userId, role));
        }
    }

    public void CreateConversation(Conversation conversation)
    {
        Conversation = conversation;
        ConversationId = conversation.Id;
    }
}

public enum CommunityType
{
    BookClub = 0,
    ReadingGroup = 1,
    GenreFans = 2,
    AuthorFans = 3,
    General = 4
}

public enum CommunityRole
{
    Member = 0,
    Moderator = 1,
    Admin = 2
}