namespace ReadingCommunityApi.Core.Models;
public class CommunityPostComment
{
    public int Id { get; private set; }
    public int PostId { get; private set; }
    public CommunityPost Post { get; private set; }

    public int AuthorId { get; private set; }
    public User Author { get; private set; }

    public string Content { get; private set; }
    public DateTime CommentedAt { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public int? ReplyToCommentId { get; private set; }
    public CommunityPostComment? ReplyToComment { get; private set; }

    private CommunityPostComment() { } // EF Core

    public CommunityPostComment(int postId, int authorId, string content)
    {
        PostId = postId;
        AuthorId = authorId;
        Content = content;
        CommentedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Edit(string content)
    {
        Content = content;
        EditedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}