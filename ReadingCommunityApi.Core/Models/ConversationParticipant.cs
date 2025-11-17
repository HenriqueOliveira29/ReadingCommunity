namespace ReadingCommunityApi.Core.Models;

public class ConversationParticipant
{
    public int Id { get; private set; }
    public int ConversationId { get; private set; }
    public Conversation Conversation { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; }
    
    public bool IsAdmin { get; private set; } // For group chats
    public DateTime JoinedAt { get; private set; }
    public DateTime? LeftAt { get; private set; }
    public DateTime? LastReadAt { get; private set; } // For unread count
    public bool IsMuted { get; private set; }
    
    private ConversationParticipant() { } // EF Core
    
    public ConversationParticipant(int conversationId, int userId, bool isAdmin = false)
    {
        ConversationId = conversationId;
        UserId = userId;
        IsAdmin = isAdmin;
        JoinedAt = DateTime.UtcNow;
        IsMuted = false;
    }
    
    public void MarkAsRead()
    {
        LastReadAt = DateTime.UtcNow;
    }
    
    public void Mute()
    {
        IsMuted = true;
    }
    
    public void Unmute()
    {
        IsMuted = false;
    }
    
    public void PromoteToAdmin()
    {
        IsAdmin = true;
    }
    
    public void DemoteFromAdmin()
    {
        IsAdmin = false;
    }
    
    public void Leave()
    {
        LeftAt = DateTime.UtcNow;
    }
}