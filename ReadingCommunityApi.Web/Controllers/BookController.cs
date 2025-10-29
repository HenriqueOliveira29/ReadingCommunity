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
    public async Task<ActionResult<PageResult<List<BookListDTO>>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await _bookService.GetAllAsync(pageIndex, pageSize));
    }
    
    [HttpGet("{id}")]
    public async  Task<ActionResult<BookDetailDTO>> GetById(int id)
    {
        return Ok(await _bookService.GetByIdAsync(id));
    }

    [HttpPost("add")]
    public async Task<ActionResult<BookDetailDTO>> Add(BookCreateDTO bookDto)
    {
        try
        {
            var result = await _bookService.AddAsync(bookDto);
            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Data); 
            }
            else
            {
                return StatusCode(
                    result.StatusCode, 
                    new 
                    { 
                        Message = result.Message,
                        Status = result.StatusCode
                    }
                );
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}