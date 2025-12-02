using System.Collections.ObjectModel;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http.Internal;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class WishlistCollectionService : IWishListCollectionService
{
    private readonly IWishListCollectionRepository _wishListCollectionRepository;

    private readonly IBookRepository _bookRepository;
    private readonly IWishListCollectionMapper _mapper;
    public WishlistCollectionService(IWishListCollectionRepository wishListCollectionRepository, IWishListCollectionMapper mapper, IBookRepository bookRepository)
    {
        _wishListCollectionRepository = wishListCollectionRepository;
        _bookRepository = bookRepository;
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
           throw new NotFoundException($"WishList can't be created");
        }

        return OperationResult.Success(
            message: "Review created successfully."
        ); 
    }

    public async Task<OperationResult> WishListCollectionAddItem(WishListItemCreateDTO dto, int userId)
    {
        var wishlistCollection = await _wishListCollectionRepository.GetWishlistCollectionById(dto.CollectionId);
        if (wishlistCollection == null)
        {
            throw new NotFoundException($"WishList with the id {dto.CollectionId} not found");
        }

        if(wishlistCollection.UserId != userId)
        {
             throw new UnauthorizedException($"WishList with the id {dto.CollectionId} doesn't exist");
        }

        var book = await _bookRepository.GetByIdAsync(dto.BookId);
        if (book == null)
        {
            throw new NotFoundException($"Book with the id {dto.BookId} not found");
        }

        wishlistCollection.AddBook(book, dto.Priority);

        await _wishListCollectionRepository.UpdateAsync(wishlistCollection);

        return OperationResult.Success(
            message: $"Item added to the WishList {wishlistCollection.Name}"
        ); 
    }
}