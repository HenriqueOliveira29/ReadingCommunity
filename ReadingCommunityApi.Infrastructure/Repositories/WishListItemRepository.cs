using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class WishListItemRepository : BaseRepository<WishlistItem>, IWishListItemRepository
{
    public WishListItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<WishlistItem>> GetWishListItemsByWishListId(int wishListId)
    {
        return await _context.WishlistItems.Include(b => b.Book).Where(w=> w.WishlistCollectionId == wishListId).ToListAsync();
    }
}