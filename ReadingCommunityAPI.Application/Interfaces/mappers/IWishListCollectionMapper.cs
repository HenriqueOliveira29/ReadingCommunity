using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IWishListCollectionMapper
{
    WishlistCollectionListDTO MapToListDto(WishlistCollection entity);
    WishlistCollectionDetailDTO MapToDetailDto(WishlistCollection entity);
    WishlistCollection MapToEntity(WishlistCollectionCreateDTO dto, int userId);

}