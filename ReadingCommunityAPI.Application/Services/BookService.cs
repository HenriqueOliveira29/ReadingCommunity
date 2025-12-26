using System.Diagnostics;
using System.Diagnostics.Metrics;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookMapper _mapper;

    private readonly ICacheService _cache;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookMapper mapper, ICacheService cache)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<OperationResult<BookDetailDTO>> AddAsync(BookCreateDTO bookDTO)
    {
        Author? author = await _authorRepository.GetByIdAsync(bookDTO.AuthorId);
        if(author == null)
        {
            throw new NotFoundException($"Cannot find the author with id {bookDTO.AuthorId}");
        }

        Book bookEntity = _mapper.MapToEntity(bookDTO);
        var result = await _bookRepository.AddAsync(bookEntity);

        await _cache.SetDataAsync<Book>($"product_{result.Id}", result);

        return OperationResult<BookDetailDTO>.Success(
            data: _mapper.MapToDetailDto(result),
            message: "Book created successfully."
        );
    }

    public async Task<OperationResult<PageResult<List<BookListDTO>>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = await _bookRepository.GetTotalCountAsync();
        int skip = (pageIndex - 1) * pageSize;
        var books = await _bookRepository.GetAllAsync(skip, pageSize);

        var data = new PageResult<List<BookListDTO>>
        {
            Items = books.Select(b => _mapper.MapToListDto(b)).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        return OperationResult<PageResult<List<BookListDTO>>>.Success(
            data,
            "Books retrieved successfully",
            statusCode: 200
        );
    }

    public async Task<OperationResult<BookDetailDTO>> GetByIdAsync(int id)
    {
        var bookCached = await _cache.GetDataAsync<Book>($"book_{id}");
        if(bookCached != null)
        {
            return OperationResult<BookDetailDTO>.Success(
                data: _mapper.MapToDetailDto(bookCached),
                message: "Book search successfully." 
            ); 
        }
        
        var result = await _bookRepository.GetByIdAsync(id);
        if (result == null)
        {
            throw new NotFoundException($"Cannot find book with id {id}");
        }
        
        return OperationResult<BookDetailDTO>.Success(
            data: _mapper.MapToDetailDto(result),
            message: "Book search successfully." 
        ); 
    }
}