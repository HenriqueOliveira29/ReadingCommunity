using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IAuthorService
{
    Task<OperationResult<PageResult<List<AuthorListDTO>>>> GetAllAsync(int pageIndex = 1, int pageSize = 0);
    Task<OperationResult<AuthorDetailDTO>> GetByIdAsync(int id);
    Task<OperationResult<AuthorDetailDTO>> AddAsync(AuthorCreateDTO author);
}