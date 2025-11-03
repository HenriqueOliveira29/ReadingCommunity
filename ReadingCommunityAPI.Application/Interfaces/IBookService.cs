using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IBookService
{
    Task<OperationResult<PageResult<List<BookListDTO>>>> GetAllAsync(int pageIndex = 1, int pageSize = 0);
    Task<OperationResult<BookDetailDTO>> GetByIdAsync(int id);
    Task<OperationResult<BookDetailDTO>> AddAsync(BookCreateDTO bookDTO);
}