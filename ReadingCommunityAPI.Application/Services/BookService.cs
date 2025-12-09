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
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _bookCounter;
    private readonly Histogram<double> _bookProcessingTime;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookMapper mapper, ActivitySource activitySource, Meter meter)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
        _activitySource = activitySource;

        _bookCounter = meter.CreateCounter<long>(
            "book.processed", 
            unit: "books",
            description: "Number of books processed");
        
        _bookProcessingTime = meter.CreateHistogram<double>(
            "book.processing.duration",
            unit: "ms",
            description: "Time taken to process books");
    }

    public async Task<OperationResult<BookDetailDTO>> AddAsync(BookCreateDTO bookDTO)
    {
        var stopwatch = Stopwatch.StartNew();
        
        using var activity = _activitySource.StartActivity("CreateBook", ActivityKind.Server);
        activity?.SetTag("book.customer_id", bookDTO.AuthorId);

        Author? author = await _authorRepository.GetByIdAsync(bookDTO.AuthorId);
        if(author == null)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Invalid author");
            throw new NotFoundException($"Cannot find the author with id {bookDTO.AuthorId}");
        }

        Book bookEntity = _mapper.MapToEntity(bookDTO);
        var result = await _bookRepository.AddAsync(bookEntity);

        activity?.AddEvent(new ActivityEvent("BookCreated",
        tags: new ActivityTagsCollection
        {
            { "book.id", bookEntity.Id },
            { "book.total", bookEntity.AuthorId }
        }));

        _bookCounter.Add(1,
                new KeyValuePair<string, object?>("status", "success"),
                new KeyValuePair<string, object?>("book_id", bookEntity.Id));

        var resultEntity = await _bookRepository.GetByIdAsync(result.Id);

        activity?.SetStatus(ActivityStatusCode.Ok);

        stopwatch.Stop();

        _bookProcessingTime.Record(stopwatch.Elapsed.TotalMilliseconds);

        return OperationResult<BookDetailDTO>.Success(
            data: _mapper.MapToDetailDto(resultEntity),
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