namespace ReadingCommunityApi.Application.Dtos.Auth;

public class AuthResponseDTO
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}