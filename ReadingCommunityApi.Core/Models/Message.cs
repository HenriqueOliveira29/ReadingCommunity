namespace ReadingCommunityApi.Core.Models;

public class Message
{
    public int Id { get; private set; }
    public int ConversationId { get; private set; }
    public Conversation Conversation { get; private set; }
    
    public int SenderId { get; private set; }
    public User Sender { get; private set; }
    
    public string Content { get; private set; }
    public MessageType Type { get; private set; }
    
    // For book/review sharing
    public int? BookId { get; private set; }
    public Book Book { get; private set; }
    
    public int? ReviewId { get; private set; }
    public Review Review { get; private set; }
    
    // For file attachments
    public string AttachmentUrl { get; private set; }
    public string AttachmentType { get; private set; } // image, pdf, etc.
    
    public DateTime SentAt { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
    // Reply to another message
    public int? ReplyToMessageId { get; private set; }
    public Message ReplyToMessage { get; private set; }
    
    public ICollection<MessageReaction> Reactions { get; private set; } = new List<MessageReaction>();
    
    private Message() { } // EF Core
    
    // Text message
    public Message(int conversationId, int senderId, string content)
    {
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        Type = MessageType.Text;
        SentAt = DateTime.UtcNow;
        IsDeleted = false;
    }
    
    // Book share message
    public Message(int conversationId, int senderId, string content, int bookId)
    {
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        BookId = bookId;
        Type = MessageType.BookShare;
        SentAt = DateTime.UtcNow;
        IsDeleted = false;
    }
    
    // File attachment message
    public Message(int conversationId, int senderId, string content, string attachmentUrl, string attachmentType)
    {
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        AttachmentUrl = attachmentUrl;
        AttachmentType = attachmentType;
        Type = MessageType.Attachment;
        SentAt = DateTime.UtcNow;
        IsDeleted = false;
    }
    
    public void Edit(string newContent)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot edit deleted message");
            
        Content = newContent;
        EditedAt = DateTime.UtcNow;
    }
    
    public void Delete()
    {
        IsDeleted = true;
        Content = "This message was deleted";
    }
    
    public void SetReplyTo(int replyToMessageId)
    {
        ReplyToMessageId = replyToMessageId;
    }
}

public enum MessageType
{
    Text = 0,
    BookShare = 1,
    ReviewShare = 2,
    Attachment = 3,
    System = 4
}