using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Book>> GetAllAsync(int skip, int take);
    Task<Book?> GetByIdAsync(int id);
}