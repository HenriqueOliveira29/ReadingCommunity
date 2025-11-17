namespace ReadingCommunityApi.Core.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public string CoverImageUrl { get; private set; }
    public ICollection<BookImage> Images { get; private set; } = new List<BookImage>();
    public int NumberOfPages { get; private set; }
    public decimal Width { get; private set; }
    public decimal Height { get; private set; }
    public decimal Depth { get; private set; }
    public int AuthorId { get; private set; }
    public Author Author { get; private set; }
    public ICollection<Category> Categories { get; private set; } = new List<Category>();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Book() { } //Empty Constructure to EF Core
    public Book(string title, string description, DateTime publicationDate, 
                int authorId, int numberOfPages, 
                decimal width, decimal height, decimal depth,
                string coverImageUrl)
    {
        Title = title;
        Description = description;
        PublicationDate = publicationDate;
        AuthorId = authorId;
        NumberOfPages = numberOfPages;
        Width = width;
        Height = height;
        Depth = depth;
        CoverImageUrl = coverImageUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddReview(Review review)
    {
        this.Reviews.Add(review);
    }

    public void AddAuthor(Author author)
    {
        this.Author = author;
    }
    
    public void AddCategory(Category category)
    {
        if (!Categories.Contains(category))
        {
            Categories.Add(category);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveCategory(Category category)
    {
        Categories.Remove(category);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCoverImage(string coverImageUrl)
    {
        CoverImageUrl = coverImageUrl;
        UpdatedAt = DateTime.UtcNow;
    }
    public string Dimensions => $"{Width} x {Height} x {Depth} cm";
}