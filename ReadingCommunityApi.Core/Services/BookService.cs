using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Core.Dtos;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Services;

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

    public async Task<Book> AddAsync(Book book)
    {
        try
        {
            Author? author= await _authorRepository.GetByIdAsync(book.AuthorId);
            if (author == null)
            {
                throw new ArgumentException($"Author {book.AuthorId} is not a valid author");
            }
            return await _bookRepository.AddAsync(book);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return book;
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

    public async Task<Book?> GetByIdAsync(int id)
    {
        try { return await _bookRepository.GetByIdAsync(id); } catch (Exception e) { Console.WriteLine(e); return null; }
    }
}