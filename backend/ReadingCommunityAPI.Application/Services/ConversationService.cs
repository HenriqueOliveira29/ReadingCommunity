using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingCommunityApi.Application.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;

        public ConversationService(IConversationRepository conversationRepository, IUserRepository userRepository)
        {
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
        }
        public async Task<Conversation?> GetConversation(int conversationId)
        {
            ArgumentNullException.ThrowIfNull(conversationId, nameof(conversationId));

            return await _conversationRepository.GetConversation(conversationId);
        }

        public async Task<OperationResult> SendMessage(int senderId, int conversationId, string content)
        {
            ArgumentNullException.ThrowIfNull(senderId, nameof(senderId));
            ArgumentNullException.ThrowIfNull(conversationId, nameof(conversationId));

            var sender = await _userRepository.GetById(senderId);
            if (sender == null)
            {
                throw new NotFoundException($"Cannot find a user with this id {senderId}");
            }

            var conversation = await _conversationRepository.GetConversation(conversationId);
            if (conversation == null)
            {
                throw new NotFoundException($"Cannot find a conversation with the id {conversationId}");
            }

            conversation.AddMessage(senderId, content);

            await _conversationRepository.UpdateAsync(conversation);

            return OperationResult.Success("Message sent successfully");
        }
    }
}
