using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Dtos.Auth;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IAuthService
{
    Task<OperationResult<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
    Task<OperationResult<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO);
}