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
        private readonly ApplicationDbContext _context;
        public ConversationRepository(ApplicationDbContext context) : base(context)
        {
         
        }
    }
}
