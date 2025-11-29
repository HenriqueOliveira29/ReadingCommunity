using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IWishListCollectionService
{
    Task<OperationResult<List<WishlistCollectionListDTO>>> GetWishListCollectionByUserId(int userId);

    Task<OperationResult> CreateWishListCollection(WishlistCollectionCreateDTO dto, int userId);

    Task<OperationResult<WishlistCollectionDetailDTO>> GetWishListCollectionById(int id, int userId);
}