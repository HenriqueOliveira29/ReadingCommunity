using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;

namespace ReadingCommunityApi.Application.Services;

public class WishlistCollectionService : IWishListCollectionService
{
    private readonly IWishListCollectionRepository _wishListCollectionRepository;
    private readonly IWishListCollectionMapper _mapper;
    public WishlistCollectionService(IWishListCollectionRepository wishListCollectionRepository, IWishListCollectionMapper mapper)
    {
        _wishListCollectionRepository = wishListCollectionRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<WishlistCollectionDetailDTO>> GetWishListCollectionById(int id, int userId)
    {
        var wishListCollection = await _wishListCollectionRepository.GetWishlistCollectionById(id);
        if (wishListCollection == null)
        {
            throw new NotFoundException($"WishList with the id {id} not found");
        }

        if(wishListCollection.UserId != userId)
        {
            throw new UnauthorizedException($"WishList with the id {id} doesn't exist");
        }

        return OperationResult<WishlistCollectionDetailDTO>.Success(
            _mapper.MapToDetailDto(wishListCollection)
        );
    }

    public async Task<OperationResult<List<WishlistCollectionListDTO>>> GetWishListCollectionByUserId(int userId)
    {
        var collections = await _wishListCollectionRepository.GetWishlistCollectionsByUser(userId);

        return OperationResult<List<WishlistCollectionListDTO>>.Success(collections.Select(t => _mapper.MapToListDto(t)).ToList());
    }

    public async Task<OperationResult> CreateWishListCollection(WishlistCollectionCreateDTO dto, int userId)
    {
        var create = await _wishListCollectionRepository.AddAsync(_mapper.MapToEntity(dto, userId));
        if (create == null)
        {
            return OperationResult.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 200
            );
        }

        return OperationResult.Success(
            message: "Review created successfully."
        ); 
    }
}