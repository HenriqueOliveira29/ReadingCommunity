using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityAPI.Application.Interfaces.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityAPI.Application.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(IConversationRepository conversation)
        {
            _conversationRepository = conversation;
        }
        public async Task<Conversation?> GetConversation(int userId, int receiverId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(receiverId, nameof(receiverId));

            return await _conversationRepository.GetConversation(userId, receiverId);
        }

        public Task<bool> SendMessage(int userId, int conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
