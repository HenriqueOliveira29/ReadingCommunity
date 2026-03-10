namespace ReadingCommunityApi.Core.Models;

public class UserFollow
{
    public int Id { get; private set; }
    
    public int FollowerId { get; private set; }
    public User Follower { get; private set; }
    
    public int FollowingId { get; private set; }
    public User Following { get; private set; }
    
    public DateTime FollowedAt { get; private set; }
    
    private UserFollow() { } // EF Core
    
    public UserFollow(int followingId, int followerId)
    {
        FollowerId = followerId;
        FollowingId = followingId;
        FollowedAt = DateTime.UtcNow;
    }
}