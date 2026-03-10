using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;


[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public async Task<ActionResult<OperationResult<AuthorDetailDTO>>> GetById(int id)
    {
        var result = await _authorService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<OperationResult<AuthorDetailDTO>>> Add(AuthorCreateDTO authorDto)
    {
        var result = await _authorService.AddAsync(authorDto);
        return StatusCode(result.StatusCode, result);
    }
}