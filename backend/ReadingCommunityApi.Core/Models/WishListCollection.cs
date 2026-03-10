namespace ReadingCommunityApi.Core.Models;
public class WishlistCollection
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsPublic { get; private set; }
    public bool IsDefault { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    public ICollection<WishlistItem> Items { get; private set; } = new List<WishlistItem>();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private WishlistCollection() { } // EF Core

    public WishlistCollection(int userId, string name, string description = null,
                              bool isPublic = false, bool isDefault = false)
    {
        UserId = userId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, bool isPublic)
    {
        Name = name;
        Description = description;
        IsPublic = isPublic;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddBook(Book book, int priority = 2, string notes = "")
    {
        var item = new WishlistItem(Id, book.Id, priority, notes);
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }
}