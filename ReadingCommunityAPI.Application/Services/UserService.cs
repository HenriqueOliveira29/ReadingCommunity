using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IUserFollowRepository _userFollowRepository;
    private readonly IUserMapper _userMapper;
    public UserService(IUserRepository userRepository, IUserFollowRepository userFollowRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userFollowRepository = userFollowRepository;
        _userMapper = userMapper;    
    }
    public async Task<OperationResult> FollowUser(int id, int userId)
    {
        var userToFollow = await _userRepository.GetById(id);
        if(userToFollow == null)
        {
            throw new NotFoundException($"User with the id {id} not found");
        }

        UserFollow userFollow = new UserFollow(id, userId);

        var result = await _userFollowRepository.AddAsync(userFollow);
        if (result == null)
        {
            throw new Exception("Cannot follow this user");
        }

        return OperationResult.Success(
            message: "Now you follow this user"
        );
    }

    public async Task<OperationResult<List<UserListDTO>>> GetAll(string? searchBy = null)
    {
        var users = await _userRepository.GetAll(searchBy);
        return OperationResult<List<UserListDTO>>.Success(users.Select(u => _userMapper.MapToListDTO(u)).ToList());
    }

    public async Task<OperationResult<UserDetailDTO>> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        if(user == null)
        {
            throw new NotFoundException("This user doesn't existe");
        }

        return OperationResult<UserDetailDTO>.Success(_userMapper.MapToDetailDTO(user));

    }

    public async Task<OperationResult> UnFollowUser(int id, int userId)
    {
        var userFollow = await _userFollowRepository.GetByIds(id, userId);
        if(userFollow == null)
        {
            throw new NotFoundException("You dont follow this user");
        }

        await _userFollowRepository.DeleteAsync(userFollow);

        return OperationResult.Success(
            message: "You unfollow this user"
        );
    }
}