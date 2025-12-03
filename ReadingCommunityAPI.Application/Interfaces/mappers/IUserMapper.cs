using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IUserMapper
{
    UserListDTO MapToListDTO(User entity);

    UserDetailDTO MapToDetailDTO(User entity);


}