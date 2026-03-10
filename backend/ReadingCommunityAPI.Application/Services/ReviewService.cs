using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class ReviewService: IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IReviewMapper _mapper;
    
    private readonly ICacheService _cache;
    public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, IReviewMapper mapper, ICacheService cache)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _cache = cache;
    }


    public async Task<OperationResult> AddReview(ReviewCreateDTO reviewCreate, int userId)
    {
        var bookCached = await _cache.GetDataAsync<Book>($"book_{reviewCreate.BookId}");
        if (bookCached == null)
        {
            Book? book = await _bookRepository.GetByIdAsync(reviewCreate.BookId);
            if (book == null)
            {
                return OperationResult.Failure(
                    message: $"Book with the id ${reviewCreate.BookId} doesnt exist",
                    statusCode: 200
                );
            }    
        }
        
        //Validar com o user logado :TODO
        Review reviewEntity = _mapper.MapToEntity(reviewCreate, userId);
        reviewEntity.SetUser(userId);

        var result = await _reviewRepository.AddAsync(reviewEntity);
        if (result == null)
        {
            return OperationResult.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 200
            );
        }

        await _cache.SetDataAsync<Review>($"review_{result.Id}", result);

        return OperationResult.Success(
            message: "Review created successfully."
        ); 
    }

    public async Task<OperationResult<List<ReviewDetailDTO>>> GetReviewsByBook(int bookId)
    {
        var reviewCached = await _cache.GetDataAsync<List<Review>>($"review_with_book_{bookId}");
        if(reviewCached != null)
        {
            return OperationResult<List<ReviewDetailDTO>>.Success(
            reviewCached.Select(r => _mapper.MapToDetailDto(r)).ToList(),
            message: "Reviews fetched successfully."
            );
        }

        var bookCached = await _cache.GetDataAsync<Book>($"book_{bookId}");
        if (bookCached == null)
        {
            Book? book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return OperationResult<List<ReviewDetailDTO>>.Failure(
                    message: $"Book with the id ${bookId} doesnt exist",
                    statusCode: 200
                );
            }
        }

        List<Review> reviews = await _reviewRepository.GetReviewByBook(bookId);
        await _cache.SetDataAsync<List<Review>>($"review_with_book_{bookId}", reviews);

        return OperationResult<List<ReviewDetailDTO>>.Success(
            reviews.Select(r => _mapper.MapToDetailDto(r)).ToList(),
            message: "Reviews fetched successfully."
        );
    }
}