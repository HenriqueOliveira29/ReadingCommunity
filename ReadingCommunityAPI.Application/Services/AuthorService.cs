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

    private readonly ICacheService _cacheService;

    public AuthorService(IAuthorRepository authorRepository, IAuthorMapper mapper, ICacheService cacheService)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
        _cacheService = cacheService;
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

        await _cacheService.SetDataAsync<Author>($"book_{resultEntity.Id}", resultEntity);
        
        return OperationResult<AuthorDetailDTO>.Success(
            data: _mapper.MapToDetailDto(resultEntity),
            message: "Author created successfully." 
        ); 
        
    }

    public async Task<OperationResult<PageResult<List<AuthorListDTO>>>> GetAllAsync(int pageIndex = 1, int pageSize = 0)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Max(1, pageSize);

        string cacheKey = $"authors_page_{pageIndex}_size_{pageSize}";

        var cachedData = await _cacheService.GetDataAsync<PageResult<List<AuthorListDTO>>>(cacheKey);

        if (cachedData != null)
        {
            return OperationResult<PageResult<List<AuthorListDTO>>>.Success(
                cachedData,
                "Authors retrieved from cache",
                statusCode: 200
            );
        }

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

        await _cacheService.SetDataAsync(cacheKey, data, TimeSpan.FromMinutes(10));

        return OperationResult<PageResult<List<AuthorListDTO>>>.Success(
            data,
            "Authors retrieved successfully",
            statusCode: 200
        );        
    }

    public async Task<OperationResult<AuthorDetailDTO>> GetByIdAsync(int id)
    {
            var authorCached = await _cacheService.GetDataAsync<Author>($"author_{id}");
            if(authorCached != null)
            {
                return OperationResult<AuthorDetailDTO>.Success(
                    data: _mapper.MapToDetailDto(authorCached),
                    message: "Book search successfully." 
                ); 
            }

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