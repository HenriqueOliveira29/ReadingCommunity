using ReadingCommunityApi.Application.Dtos;
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
        Task<OperationResult> SendMessage(int senderId, int conversationId, string content);

        Task<Conversation?> GetConversation(int userId, int receiverId);
    }
}
