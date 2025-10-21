using ReadingCommunityApi.Core.Dto;
using ReadingCommunityApi.Core.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IAuthorService
{
    Task<PageResult<List<AuthorListDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 0);
    Task<Author?> GetByIdAsync(int id);
    Task<OperationResult<AuthorDetailDTO>> AddAsync(AuthorCreateDTO author);
}