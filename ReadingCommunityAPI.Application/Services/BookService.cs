using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    private readonly ILogger<BookService> _logger;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper, ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<BookDetailDTO>> AddAsync(BookCreateDTO bookDTO)
    {
        try
        {
            Book bookEntity = _mapper.Map<Book>(bookDTO);
            var result = await _bookRepository.AddAsync(bookEntity);
            if (result == null)
            {
                return OperationResult<BookDetailDTO>.Failure(
                    message: "An unexpected server error occurred.",
                    statusCode: 200
                );
            }

            var resultEntity = await _bookRepository.GetByIdAsync(result.Id);
            if (resultEntity == null)
            {
                return OperationResult<BookDetailDTO>.Failure(
                    message: $"Cannot get the Book with id {result.Id}",
                    statusCode: 200
                );
            }
            
            return OperationResult<BookDetailDTO>.Success(
                data: _mapper.Map<BookDetailDTO>(resultEntity),
                message: "Book created successfully." 
            ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a book.");
            return OperationResult<BookDetailDTO>.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500 
            );
        }
    }

    public async Task<PageResult<List<BookListDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 0)
    {
        try
        {
            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Max(1, pageSize);

            int totalCount = await _bookRepository.GetTotalCountAsync();

            int skip = (pageIndex - 1) * pageSize;

            var authors = await _bookRepository.GetAllAsync(skip, pageSize);

            return new PageResult<List<BookListDTO>>
            {
                Items = _mapper.Map<List<BookListDTO>>(authors),
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching all books.");
            throw new Exception("Could not retrieve books due to an internal error.", ex);
        }
    }

    public async Task<OperationResult<BookDetailDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await _bookRepository.GetByIdAsync(id);
            if (result == null)
            {
                return OperationResult<BookDetailDTO>.Failure(
                    message: $"Cannot get the Book with id {id}",
                    statusCode: 200
                );
            }
            
            return OperationResult<BookDetailDTO>.Success(
                data: _mapper.Map<BookDetailDTO>(result),
                message: "Book search successfully." 
            ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while get the book with id {id}.");
            return OperationResult<BookDetailDTO>.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500 
            );
        }
    }
}