using ReadingCommunityApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityApi.Core.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetConversation(int userId, int receiverId);

        bool SendMessage(int userId, int conversationId);
    }
}
