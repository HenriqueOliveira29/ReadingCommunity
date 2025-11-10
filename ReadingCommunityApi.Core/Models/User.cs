using Microsoft.AspNetCore.Identity;

namespace ReadingCommunityApi.Core.Models;

public class User : IdentityUser<int>
{
   public DateTime CreatedAt { get; set; }
   public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public User(string username, string email)
    {
        this.UserName = username;
        this.Email = email;
        this.CreatedAt = DateTime.UtcNow;
    }

    public void AddReview(Review review)
    {
        this.Reviews.Add(review);
    }
}