using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IReviewService
{
    Task<OperationResult> AddReview(ReviewCreateDTO reviewCreate);

    Task<OperationResult<List<ReviewDetailDTO>>> GetReviewsByBook(int bookId);
}