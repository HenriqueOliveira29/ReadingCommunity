using ReadingCommunityApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityApi.Core.Interfaces
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        Task<Conversation?> GetConversation(int conversationId);

        Task<List<Conversation>> GetUserConversations(int userId);

        Task<Conversation?> GetConversationBetweenUsers(int userId1, int userId2);
    }
}
