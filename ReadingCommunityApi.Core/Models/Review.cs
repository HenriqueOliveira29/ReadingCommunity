namespace ReadingCommunityApi.Core.Models;

public class Review
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime DatePosted { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public Review(int rating, string comment, Book book, User user)
    {
        this.Rating = rating;
        this.Comment = comment;
        this.Book = book;
        this.User = user;
        this.DatePosted = DateTime.Now;
    }
    
    private Review() {} //Empty Constructure to EF Core
}