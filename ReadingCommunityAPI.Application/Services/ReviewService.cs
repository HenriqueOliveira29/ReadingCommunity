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
    private readonly ILogger<ReviewService> _logger;
    public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, IReviewMapper mapper, ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<OperationResult> AddReview(ReviewCreateDTO reviewCreate, int userId)
    {
        try
        {
            Book? book = await _bookRepository.GetByIdAsync(reviewCreate.BookId);
            if (book == null)
            {
                return OperationResult.Failure(
                   message: $"Book with the id ${reviewCreate.BookId} doesnt exist",
                   statusCode: 200
               );
            }

            //Validar com o user logado :TODO
            Review reviewEntity = _mapper.MapToEntity(reviewCreate, userId);
            reviewEntity.SetUser(userId);

            Console.WriteLine($" lets sse {reviewEntity}");
            var result = await _reviewRepository.AddAsync(reviewEntity);
            if (result == null)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a review.");
            return OperationResult.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500
            );
        }
    }

    public async Task<OperationResult<List<ReviewDetailDTO>>> GetReviewsByBook(int bookId)
    {
        try
        {
            Book? book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return OperationResult<List<ReviewDetailDTO>>.Failure(
                   message: $"Book with the id ${bookId} doesnt exist",
                   statusCode: 200
               );
            }

            List<Review> reviews = await _reviewRepository.GetReviewByBook(bookId);
            return OperationResult<List<ReviewDetailDTO>>.Success(
                reviews.Select(r => _mapper.MapToDetailDto(r)).ToList(),
                message: "Reviews fetched successfully."
            );
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a review.");
            return OperationResult<List<ReviewDetailDTO>>.Failure(
                message: "An unexpected server error occurred.",
                statusCode: 500
            );
        }
    }
}