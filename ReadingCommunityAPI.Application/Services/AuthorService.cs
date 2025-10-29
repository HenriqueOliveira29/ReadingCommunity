using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<AuthorService> _logger;
    private readonly IMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthorDetailDTO>> AddAsync(AuthorCreateDTO author)
    {
        try
        {
            Author authorEntity = _mapper.Map<Author>(author);
            var result = await _authorRepository.AddAsync(authorEntity);
            if (result == null)
            {
                return OperationResult<AuthorDetailDTO>.Failure(
                    message: "An unexpected server error occurred.",
                    statusCode: 200
                );
            }

            var resultEntity = await _authorRepository.GetByIdAsync(result.Id);
            if (resultEntity == null)
            {
                return OperationResult<AuthorDetailDTO>.Failure(
                    message: $"Cannot get the Author with id {result.Id}",
                    statusCode: 200
                );
            }
            
            return OperationResult<AuthorDetailDTO>.Success(
                data: _mapper.Map<AuthorDetailDTO>(resultEntity),
                message: "Author created successfully." 
            ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a author.");
            return OperationResult<AuthorDetailDTO>.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500 
            );
        }
    }

    public async Task<PageResult<List<AuthorListDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 0)
    {
        try
        {
            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Max(1, pageSize);

            int totalCount = await _authorRepository.GetTotalCountAsync();

            int skip = (pageIndex - 1) * pageSize;

            var authorEntities = await _authorRepository.GetAllAsync(skip, pageSize);

            var authorsDTO = _mapper.Map<List<AuthorListDTO>>(authorEntities);

            return new PageResult<List<AuthorListDTO>>
            {
                Items = authorsDTO,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching all authors.");
            throw new Exception("Could not retrieve authors due to an internal error.", ex);
        }
    }

    public async Task<Author?> Existe(int id)
    {
        try
        {
            return await _authorRepository.Exist(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<OperationResult<AuthorDetailDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await _authorRepository.GetByIdAsync(id);
            if (result == null)
            {
                return OperationResult<AuthorDetailDTO>.Failure(
                    message: $"Cannot get the author with id {id}",
                    statusCode: 200
                );
            }
            
            return OperationResult<AuthorDetailDTO>.Success(
                data: _mapper.Map<AuthorDetailDTO>(result),
                message: "Author search successfully." 
            ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while get the author with id {id}.");
            return OperationResult<AuthorDetailDTO>.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500 
            );
        }
    }
}