using Microsoft.VisualBasic;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IWishListCollectionRepository : IBaseRepository<WishlistCollection>
{
    Task<List<WishlistCollection>> GetWishlistCollectionsByUser(int userId);

    Task<WishlistCollection> GetWishlistCollectionById(int id);
}