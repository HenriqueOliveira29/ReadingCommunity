using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IAuthorRepository : IBaseRepository<Author>
{
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Author>> GetAllAsync(int skip, int take);
    Task<Author?> GetByIdAsync(int id);
    Task<Author?> Exist(int id);
}