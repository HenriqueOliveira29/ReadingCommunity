namespace ReadingCommunityApi.Core.Models;

public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }

    public string Email { get; private set; }

    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public User(string username, string email)
    {
        this.Username = username;
        this.Email = email;
    }

    public void AddReview(Review review)
    {
        this.Reviews.Add(review);
    }
}