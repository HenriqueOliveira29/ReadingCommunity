using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserListDTO>>> GetAll(
        [FromQuery] string searchBy = "")
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return Ok(await _userService.GetAll(userId, searchBy == "" ? null: searchBy));
    }
    
    [HttpGet("{id}")]
    public async  Task<ActionResult<UserDetailDTO>> GetById(int id)
    {
        var result = await _userService.GetById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("followUser/{id}")]
    public async Task<ActionResult> FollowUser(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _userService.FollowUser(id, userId);
        return StatusCode(result.StatusCode, result);   
    }

    [HttpGet("unFollowUser/{id}")]
    public async Task<ActionResult> UnFollowUser(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _userService.UnFollowUser(id, userId);
        return StatusCode(result.StatusCode, result);   
    }
}