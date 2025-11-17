namespace ReadingCommunityApi.Core.Models;

public class MessageReaction
{
    public int Id { get; private set; }
    public int MessageId { get; private set; }
    public Message Message { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; }
    
    public string Emoji { get; private set; }
    public DateTime ReactedAt { get; private set; }
    
    private MessageReaction() { } // EF Core
    
    public MessageReaction(int messageId, int userId, string emoji)
    {
        MessageId = messageId;
        UserId = userId;
        Emoji = emoji;
        ReactedAt = DateTime.UtcNow;
    }
}