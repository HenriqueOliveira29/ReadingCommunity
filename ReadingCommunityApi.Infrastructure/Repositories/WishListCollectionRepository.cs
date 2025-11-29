using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class WishListCollectionReposotory : BaseRepository<WishlistCollection> ,IWishListCollectionRepository
{
    public WishListCollectionReposotory(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<WishlistCollection?> GetWishlistCollectionById(int Id)
    {
        return await _context.WishlistCollections.Include(w => w.Items).Where(u => u.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<List<WishlistCollection>> GetWishlistCollectionsByUser(int userId)
    {
        return await _context.WishlistCollections.Where(u => u.UserId == userId).ToListAsync();
    }
}