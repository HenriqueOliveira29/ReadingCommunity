// Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Dtos.Auth;
using ReadingCommunityApi.Application.Interfaces;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<OperationResult<AuthResponseDTO>>> Register(RegisterDTO registerDTO)
    {
        var result = await _authService.RegisterAsync(registerDTO);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<OperationResult<AuthResponseDTO>>> Login(LoginDTO loginDTO)
    {
        var result = await _authService.LoginAsync(loginDTO);
        return StatusCode(result.StatusCode, result);
    }
}