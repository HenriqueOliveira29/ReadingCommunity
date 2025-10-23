using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;

    }

    [HttpGet]
    public async  Task<ActionResult<PageResult<List<BookListDTO>>>> GetAll(
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 10)
    {
        return Ok(await _bookService.GetAllAsync(pageIndex, pageSize));
    }

    // [HttpPost("add")]
    // public async Task<ActionResult<Book>> Add(BookCreateDto book)
    // {
    //     try
    //     {
    //         var newBook = await _bookService.AddAsync(originalBook);
    //         return Ok(newBook);
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
}