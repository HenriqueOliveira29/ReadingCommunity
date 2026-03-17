using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityApi.Application.Interfaces
{
    public interface IConversationService
    {
        Task<OperationResult> SendMessage(int senderId, int conversationId, string content);

        Task<Conversation?> GetConversation(int conversationId);

        Task<OperationResult<List<Conversation>>> GetUserConversations(int userId);

        Task<OperationResult<Conversation>> CreateConversation(int userId, int otherUserId);
    }
}
