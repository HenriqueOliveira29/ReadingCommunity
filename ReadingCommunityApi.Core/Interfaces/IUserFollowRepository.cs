using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IUserFollowRepository: IBaseRepository<UserFollow>
{
    Task<UserFollow> GetByIds(int followingId, int followerId);
}