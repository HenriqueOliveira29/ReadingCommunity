using Microsoft.AspNetCore.Identity;

namespace ReadingCommunityApi.Core.Models;

public class User : IdentityUser<int>
{
    public string Bio { get; private set; }
    public string ProfileImageUrl { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<WishlistCollection> WishlistCollections { get; private set; } = new List<WishlistCollection>();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    public ICollection<UserFollow> Followers { get; private set; } = new List<UserFollow>();
    public ICollection<UserFollow> Following { get; private set; } = new List<UserFollow>();

    
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
    public User(){}
}