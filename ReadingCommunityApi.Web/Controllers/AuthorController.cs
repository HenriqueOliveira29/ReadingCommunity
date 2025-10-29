using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;


[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResult<List<AuthorListDTO>>>> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await _authorService.GetAllAsync(pageIndex, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDetailDTO>> GetById(int id)
    {
        var result = await _authorService.GetByIdAsync(id);

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

    [HttpPost("add")]
    public async Task<ActionResult<AuthorDetailDTO>> Add(AuthorCreateDTO authorDto)
    {
        var result = await _authorService.AddAsync(authorDto);

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
}