using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Interfaces;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<List<Review>> GetReviewByBook(int bookId);   
}