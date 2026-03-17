using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityApi.Infrastructure.Repositories
{
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(ApplicationDbContext context) : base(context)
        {
         
        }

        public async Task<Conversation?> GetConversation(int conversationId)
        {
            return await _context.Conversations.Include(c => c.Participants).FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<List<Conversation>> GetUserConversations(int userId)
        {
            return await _context.Conversations
                .Include(c => c.Participants)
                .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .ToListAsync();
        }

        public async Task<Conversation?> GetConversationBetweenUsers(int userId1, int userId2)
        {
            return await _context.Conversations
                .Include(c => c.Participants)
                .Where(c => c.Participants.Any(p => p.UserId == userId1) && c.Participants.Any(p => p.UserId == userId2))
                .FirstOrDefaultAsync();
        }
    }
}
