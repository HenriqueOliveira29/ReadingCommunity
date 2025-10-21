using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<int> GetTotalCountAsync()
    {
        return _context.Authors.CountAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAsync(int skip, int take)
    {
        return await _context.Authors.OrderBy(a => a.Id).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Author?> Exist(int id)
    {
        return await _context.Authors.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.Include(x => x.Books).Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}