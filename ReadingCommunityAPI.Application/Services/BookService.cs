using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<BookDetailDTO>> AddAsync(BookCreateDTO bookDTO)
    {
        Author? author = await _authorRepository.GetByIdAsync(bookDTO.AuthorId);
        if(author == null)
        {
            throw new NotFoundException($"Cannot find the author with id {bookDTO.AuthorId}");
        }

        Book bookEntity = _mapper.Map<Book>(bookDTO);
        var result = await _bookRepository.AddAsync(bookEntity);

        var resultEntity = await _bookRepository.GetByIdAsync(result.Id);

        return OperationResult<BookDetailDTO>.Success(
            data: _mapper.Map<BookDetailDTO>(resultEntity),
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
            Items = _mapper.Map<List<BookListDTO>>(books),
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
            data: _mapper.Map<BookDetailDTO>(result),
            message: "Book search successfully." 
        ); 
    }
}