using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.Extensions.Logging;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Services;

public class ReviewService: IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ReviewService> _logger;
    public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, IMapper mapper, ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<OperationResult> AddReview(ReviewCreateDTO reviewCreate)
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
            Review reviewEntity = _mapper.Map<Review>(reviewCreate);

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


}