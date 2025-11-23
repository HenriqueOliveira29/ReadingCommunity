namespace ReadingCommunityApi.Core.Models;
public class CommunityPost
{
    public int Id { get; private set; }
    public int CommunityId { get; private set; }
    public Community Community { get; private set; }

    public int AuthorId { get; private set; }
    public User Author { get; private set; }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public PostType Type { get; private set; }
    public int? BookId { get; private set; }
    public Book? Book { get; private set; }

    public DateTime PostedAt { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsPinned { get; private set; }

    public ICollection<CommunityPostComment> Comments { get; private set; } = new List<CommunityPostComment>();
    public ICollection<CommunityPostReaction> Reactions { get; private set; } = new List<CommunityPostReaction>();

    private CommunityPost() { } // EF Core

    public CommunityPost(int communityId, int authorId, string title, string content, PostType type)
    {
        CommunityId = communityId;
        AuthorId = authorId;
        Title = title;
        Content = content;
        Type = type;
        PostedAt = DateTime.UtcNow;
        IsDeleted = false;
        IsPinned = false;
    }

    public void Edit(string title, string content)
    {
        Title = title;
        Content = content;
        EditedAt = DateTime.UtcNow;
    }

    public void Pin()
    {
        IsPinned = true;
    }

    public void Unpin()
    {
        IsPinned = false;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

public enum PostType
{
    Discussion = 0,
    Question = 1,
    Recommendation = 2,
    Review = 3,
    Announcement = 4
}