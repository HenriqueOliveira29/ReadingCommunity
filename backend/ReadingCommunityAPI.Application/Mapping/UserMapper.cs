using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class UserMapper : IUserMapper
{
    private readonly IWishListCollectionMapper _wishListCollectionMapper;
    public UserMapper(IWishListCollectionMapper wishListCollectionMapper)
    {
        _wishListCollectionMapper = wishListCollectionMapper;
    }
    public UserDetailDTO MapToDetailDTO(User entity)
    {
        return new UserDetailDTO()
        {
            Id = entity.Id,
            UserName = entity.UserName,
            ProfileImageUrl = entity.ProfileImageUrl,
            NumberFollowers = entity.Followers.Count(),
            NumberFollowing = entity.Following.Count(),
            WishLists = entity.WishlistCollections.Select(wi => _wishListCollectionMapper.MapToListDto(wi)).ToList()
        };
    }

    public UserListDTO MapToListDTO(User entity)
    {
        return new UserListDTO()
        {
            Id = entity.Id,
            UserName = entity.UserName,
            ProfileImageUrl = entity.ProfileImageUrl
        };
    }
}