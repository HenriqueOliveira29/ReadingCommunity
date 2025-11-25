using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IAuthorMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, IAuthorMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthorDetailDTO>> AddAsync(AuthorCreateDTO author)
    {
        Author authorEntity = _mapper.MapToEntity(author);
        var result = await _authorRepository.AddAsync(authorEntity);

        var resultEntity = await _authorRepository.GetByIdAsync(result.Id);
        if (resultEntity == null)
        {
            throw new NotFoundException($"Cannot find the author with id {resultEntity?.Id}");
        }
        
        return OperationResult<AuthorDetailDTO>.Success(
            data: _mapper.MapToDetailDto(resultEntity),
            message: "Author created successfully." 
        ); 
        
    }

    public async Task<OperationResult<PageResult<List<AuthorListDTO>>>> GetAllAsync(int pageIndex = 1, int pageSize = 0)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Max(1, pageSize);

        int totalCount = await _authorRepository.GetTotalCountAsync();

        int skip = (pageIndex - 1) * pageSize;

        var authorEntities = await _authorRepository.GetAllAsync(skip, pageSize);

        var data = new PageResult<List<AuthorListDTO>>
        {
            Items = authorEntities.Select(a => _mapper.MapToListDto(a)).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        return OperationResult<PageResult<List<AuthorListDTO>>>.Success(
            data,
            "Authors retrieved successfully",
            statusCode: 200
        );        
    }

    public async Task<OperationResult<AuthorDetailDTO>> GetByIdAsync(int id)
    {
            var result = await _authorRepository.GetByIdAsync(id);
            if (result == null)
            {
                throw new NotFoundException($"Cannot find the author with id {id}");
            }
            
            return OperationResult<AuthorDetailDTO>.Success(
                data: _mapper.MapToDetailDto(result),
                message: "Author search successfully." 
            ); 
    }
}