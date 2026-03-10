using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Book>> GetAllAsync(int skip, int take)
    {
        return await _context.Books.Include(b => b.Author).OrderBy(a => a.Id).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.Include(t => t.Author).Where(t => t.Id == id).FirstOrDefaultAsync();
    }

    public Task<int> GetTotalCountAsync()
    {
         return _context.Books.CountAsync();
    }
}