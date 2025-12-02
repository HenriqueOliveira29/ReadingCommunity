using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IUserService
{
    Task<OperationResult<List<UserListDTO>>> GetAll(string? searchBy = null);

    Task<OperationResult<UserDetailDTO>> GetById(int id);

    Task<OperationResult> FollowUser(int id, int userId);

    Task<OperationResult> UnFollowUser(int id, int userId);
}