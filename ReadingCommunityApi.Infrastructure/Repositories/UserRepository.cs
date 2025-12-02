using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;

namespace ReadingCommunityApi.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context): base(context)
    {
        
    }
    public async Task<List<User>> GetAll(string? searchBy = null)
    {
        var query = _context.Users.AsQueryable();

        if(searchBy != null)
        {
            query = query.Where(u => u.UserName.ToLower().Contains(searchBy.ToLower()));
        }

        return await query.ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.Include(u=> u.Followers).Include(uf => uf.Following).Include(uwi => uwi.WishlistCollections).Where(us => us.Id == id).FirstOrDefaultAsync();
    }
}