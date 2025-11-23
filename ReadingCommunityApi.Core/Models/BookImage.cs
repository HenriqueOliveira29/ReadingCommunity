namespace ReadingCommunityApi.Core.Models;
public class BookImage
{
    public int Id { get; private set; }
    public string ImageUrl { get; private set; }
    public string Description { get; private set; }
    public int BookId { get; private set; }
    public Book Book { get; private set; }

    private BookImage() { }

    public BookImage(string imageUrl, string description, int bookId)
    {
        ImageUrl = imageUrl;
        Description = description;
        BookId = bookId;
    }
}