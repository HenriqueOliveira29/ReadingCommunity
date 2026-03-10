using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class UserFollowRepository : BaseRepository<UserFollow>, IUserFollowRepository
{
    public UserFollowRepository(ApplicationDbContext context): base(context)
    {
        
    }

    public async Task<UserFollow?> GetByIds(int followingId, int followerId)
    {
        return await _context.UserFollows.Where(u=> u.FollowingId == followingId).Where(uf => uf.FollowerId == followerId).FirstOrDefaultAsync();
    }
}