namespace ReadingCommunityApi.Core.Models;
public class CommunityPostReaction
{
    public int Id { get; private set; }
    public int PostId { get; private set; }
    public CommunityPost Post { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    public string Emoji { get; private set; }
    public DateTime ReactedAt { get; private set; }

    private CommunityPostReaction() { } // EF Core

    public CommunityPostReaction(int postId, int userId, string emoji)
    {
        PostId = postId;
        UserId = userId;
        Emoji = emoji;
        ReactedAt = DateTime.UtcNow;
    }
}