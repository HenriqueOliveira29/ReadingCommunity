using Microsoft.VisualBasic;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IWishListItemRepository : IBaseRepository<WishlistItem>
{
    Task<List<WishlistItem>> GetWishListItemsByWishListId(int WishlistId);
}