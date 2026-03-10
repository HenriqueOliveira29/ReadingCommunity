using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class WishListItemMapper : IWishListItemMapper
{
    private readonly IBookMapper _bookMapper;
    public WishListItemMapper(IBookMapper bookMapper)
    {
        _bookMapper = bookMapper;
    }
    public WishListItemListDTO MapToListDto(WishlistItem entity)
    {
        if(entity.Book == null)
        {
            throw new NotFoundException("This item doesnt have a book");
        }

        return new WishListItemListDTO
        {
            Id = entity.Id,
            Book = _bookMapper.MapToListDto(entity.Book),
            AddedAt = entity.AddedAt,
            Priority = entity.Priority,
            IsPurchased = entity.IsPurchased
        };
    }
}