namespace ReadingCommunityApi.Core.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime PublictionDate { get; private set; }
    public int AuthorId { get; private set; }
    public Author Author { get; private set; }
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    public Book(string title, string description, DateTime publictionDate, Author author)
    {
        this.Title = title;
        this.Description = description;
        this.PublictionDate = publictionDate;
        this.Author = author;
        this.AuthorId = author.Id;
    }

    public void AddReview(Review review)
    {
        this.Reviews.Add(review);
    }
    
    public void AddAuthor(Author author)
    {
        this.Author = author;
    }

    private Book() { } //Empty Constructure to EF Core
}