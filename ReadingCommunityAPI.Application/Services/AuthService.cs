using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Dtos.Auth;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtService _jwtService;

    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtService jwtService, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<OperationResult<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDTO)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(registerDTO.Username))
            return OperationResult<AuthResponseDTO>.Failure("Username is required", 400);

        if (string.IsNullOrWhiteSpace(registerDTO.Email))
            return OperationResult<AuthResponseDTO>.Failure("Email is required", 400);

        if (string.IsNullOrWhiteSpace(registerDTO.Password))
            return OperationResult<AuthResponseDTO>.Failure("Password is required", 400);

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (existingUser != null)
            return OperationResult<AuthResponseDTO>.Failure("Email already registered", 409);

        // Create user
        var user = new User(registerDTO.Username, registerDTO.Email);

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return OperationResult<AuthResponseDTO>.Failure(errors, 400);
        }

        // Generate token
        var token = _jwtService.GenerateToken(user);
        var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"]);

        var response = new AuthResponseDTO
        {
            UserId = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };

        return OperationResult<AuthResponseDTO>.Success(response, "Registration successful", 201);
    }

    public async Task<OperationResult<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(loginDTO.Email))
            return OperationResult<AuthResponseDTO>.Failure("Email is required", 400);

        if (string.IsNullOrWhiteSpace(loginDTO.Password))
            return OperationResult<AuthResponseDTO>.Failure("Password is required", 400);

        // Get user
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
            return OperationResult<AuthResponseDTO>.Failure("Invalid email or password", 401);

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
            return OperationResult<AuthResponseDTO>.Failure("Invalid email or password", 401);

        // Generate token
        var token = _jwtService.GenerateToken(user);
        var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"]);

        var response = new AuthResponseDTO
        {
            UserId = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };

        return OperationResult<AuthResponseDTO>.Success(response, "Login successful", 200);
    }
}