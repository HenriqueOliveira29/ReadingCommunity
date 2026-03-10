namespace ReadingCommunityApi.Core.Models;
public class CommunityMember
{
    public int Id { get; private set; }
    public int CommunityId { get; private set; }
    public Community Community { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    public CommunityRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public DateTime? LeftAt { get; private set; }

    private CommunityMember() { } // EF Core

    public CommunityMember(int communityId, int userId, CommunityRole role = CommunityRole.Member)
    {
        CommunityId = communityId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    public void PromoteTo(CommunityRole newRole)
    {
        Role = newRole;
    }

    public void Leave()
    {
        LeftAt = DateTime.UtcNow;
    }
}