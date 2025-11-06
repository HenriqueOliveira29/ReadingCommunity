using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthorDetailDTO>> AddAsync(AuthorCreateDTO author)
    {
        Author authorEntity = _mapper.Map<Author>(author);
        var result = await _authorRepository.AddAsync(authorEntity);

        var resultEntity = await _authorRepository.GetByIdAsync(result.Id);
        if (resultEntity == null)
        {
            throw new NotFoundException($"Cannot find the author with id {resultEntity?.Id}");
        }
        
        return OperationResult<AuthorDetailDTO>.Success(
            data: _mapper.Map<AuthorDetailDTO>(resultEntity),
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

        var authorsDTO = _mapper.Map<List<AuthorListDTO>>(authorEntities);

        var data = new PageResult<List<AuthorListDTO>>
        {
            Items = authorsDTO,
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
                data: _mapper.Map<AuthorDetailDTO>(result),
                message: "Author search successfully." 
            ); 
    }
}