namespace ReadingCommunityApi.Core.Models;

public class Review
{
    public int Id { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; }

    public DateTime DatePosted { get; private set; }

    public int BookId { get; private set; }
    public Book Book { get;  private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    public ICollection<Message> MessagesReference {get; private set;} = new List<Message>();

    public Review(int rating, string comment, Book book, User user)
    {
        this.Rating = rating;
        this.Comment = comment;
        this.Book = book;
        this.User = user;
        this.DatePosted = DateTime.Now;
    }

    public void SetUser(int userId)
    {
        this.UserId = userId;
    }
    
    private Review() {} //Empty Constructure to EF Core
}