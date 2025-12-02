using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class WishListCollectionMapper : IWishListCollectionMapper
{
    private readonly IWishListItemMapper _wishListItemMapper;
    public WishListCollectionMapper(IWishListItemMapper wishListItemMapper)
    {
        _wishListItemMapper = wishListItemMapper;
    }
    public WishlistCollectionDetailDTO MapToDetailDto(WishlistCollection entity)
    {

        return new WishlistCollectionDetailDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsPublic = entity.IsPublic,
            IsDefault = entity.IsDefault,
            Items = entity.Items.Select(w => _wishListItemMapper.MapToListDto(w)).ToList()
        };
    }

    public WishlistCollection MapToEntity(WishlistCollectionCreateDTO dto, int userId)
    {
       return new WishlistCollection(userId, dto.Name, dto.Description, dto.isPublic, dto.isDefault);
    }

    public WishlistCollectionListDTO MapToListDto(WishlistCollection entity)
    {
        return new WishlistCollectionListDTO
        {
          Id=entity.Id,
          Name= entity.Name,
          Description = entity.Description,
          IsPublic = entity.IsPublic,
          NumberOfItems = entity.Items.Count()
        };
    }
}