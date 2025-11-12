using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IReviewService
{
    Task<OperationResult> AddReview(ReviewCreateDTO reviewCreate, int userId);

    Task<OperationResult<List<ReviewDetailDTO>>> GetReviewsByBook(int bookId);
}