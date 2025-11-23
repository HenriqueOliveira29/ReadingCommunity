namespace ReadingCommunityApi.Core.Models;
public class Conversation
{
    public int Id { get; private set; }
    public ConversationType Type { get; private set; } // Direct or Group
    public string Name { get; private set; } // For group chats
    public string Description { get; private set; }
    public string ImageUrl { get; private set; } // Group chat image
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastMessageAt { get; private set; }
    
    public ICollection<ConversationParticipant> Participants { get; private set; } = new List<ConversationParticipant>();
    public ICollection<Message> Messages { get; private set; } = new List<Message>();
    
    private Conversation() { } // EF Core
    
    // Direct message constructor
    public Conversation(int user1Id, int user2Id)
    {
        Type = ConversationType.Direct;
        CreatedAt = DateTime.UtcNow;
        
        Participants.Add(new ConversationParticipant(Id, user1Id));
        Participants.Add(new ConversationParticipant(Id, user2Id));
    }
    
    // Group chat constructor
    public Conversation(string name, string description, int creatorId, string imageUrl = null)
    {
        Type = ConversationType.Group;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
        
        Participants.Add(new ConversationParticipant(Id, creatorId, isAdmin: true));
    }
    
    public void UpdateLastMessageTime()
    {
        LastMessageAt = DateTime.UtcNow;
    }
    
    public void UpdateGroupInfo(string name, string description, string imageUrl)
    {
        if (Type != ConversationType.Group)
            throw new InvalidOperationException("Can only update group conversations");
            
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }
    
    public void AddParticipant(int userId, bool isAdmin = false)
    {
        if (Type != ConversationType.Group)
            throw new InvalidOperationException("Can only add participants to group conversations");
            
        if (!Participants.Any(p => p.UserId == userId))
        {
            Participants.Add(new ConversationParticipant(Id, userId, isAdmin));
        }
    }
    
    public void RemoveParticipant(int userId)
    {
        var participant = Participants.FirstOrDefault(p => p.UserId == userId);
        if (participant != null)
        {
            Participants.Remove(participant);
        }
    }
}

public enum ConversationType
{
    Direct = 0,
    Group = 1
}