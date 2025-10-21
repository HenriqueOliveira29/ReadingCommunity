using ReadingCommunityApi.Core.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IBookService
{
    Task<PageResult<List<BookListDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 0);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> AddAsync(Book book);
}