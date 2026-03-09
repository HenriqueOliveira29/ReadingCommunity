using ReadingCommunityApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityAPI.Application.Interfaces.services
{
    public interface IConversationService
    {
        Task<bool> SendMessage(int userId, int conversationId);

        Task<Conversation?> GetConversation(int userId, int receiverId);
    }
}
