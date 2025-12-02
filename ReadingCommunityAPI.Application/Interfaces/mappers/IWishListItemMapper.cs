using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IWishListItemMapper
{
    WishListItemListDTO MapToListDto(WishlistItem entity);
}