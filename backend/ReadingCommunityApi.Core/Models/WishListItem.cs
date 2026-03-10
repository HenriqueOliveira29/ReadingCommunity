namespace ReadingCommunityApi.Core.Models;

public class WishlistItem
{
    public int Id { get; private set; }
    public int WishlistCollectionId { get; private set; }
    public WishlistCollection WishlistCollection { get; private set; }
    
    public int BookId { get; private set; }
    public Book Book { get; private set; }
    
    public DateTime AddedAt { get; private set; }
    public int Priority { get; private set; }
    public string Notes { get; private set; }
    public bool IsPurchased { get; private set; }
    
    private WishlistItem() { } // EF Core
    
    public WishlistItem(int wishlistCollectionId, int bookId, int priority = 2, string notes = null)
    {
        WishlistCollectionId = wishlistCollectionId;
        BookId = bookId;
        Priority = priority;
        Notes = notes;
        AddedAt = DateTime.UtcNow;
        IsPurchased = false;
        SetPriority(priority);
    }
    
    public void SetPriority(int priority)
    {
        if (priority < 1 || priority > 3)
            throw new ArgumentException("Priority must be between 1 and 3");
        Priority = priority;
    }
    
    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }
    
    public void MarkAsPurchased()
    {
        IsPurchased = true;
    }
    
    public void MarkAsUnpurchased()
    {
        IsPurchased = false;
    }
}