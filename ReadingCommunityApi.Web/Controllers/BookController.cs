using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;

    }

    [HttpGet]
    public async Task<ActionResult<PageResult<List<BookListDTO>>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await _bookService.GetAllAsync(pageIndex, pageSize));
    }
    
    [HttpGet("{id}")]
    public async  Task<ActionResult<BookDetailDTO>> GetById(int id)
    {
        var result = await _bookService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<BookDetailDTO>> Add(BookCreateDTO bookDto)
    {
        var result = await _bookService.AddAsync(bookDto);
        return StatusCode(result.StatusCode, result);   
    }
}