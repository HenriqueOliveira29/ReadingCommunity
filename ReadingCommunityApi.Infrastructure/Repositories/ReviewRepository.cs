using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class ReviewRepository : BaseRepository<Review> , IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<List<Review>> GetReviewByBook(int bookId)
    {
        return await _context.Reviews.Where(r => r.BookId == bookId).ToListAsync();
    }
}