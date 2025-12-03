using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<List<User>> GetAll(int userId, string? searchBy = null);

    public Task<User?> GetById(int id);
}