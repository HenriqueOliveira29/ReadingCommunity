using Microsoft.AspNetCore.SignalR;
using ReadingCommunityApi.Application.Interfaces;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;
using System.Security.Claims;

namespace ReadingCommunityApi.Web
{
    public class ChatHub : Hub
    {
        private readonly IConversationService _conversationService;

        public ChatHub(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                conversationId);
        }

        public async Task SendMessage(string conversationId, string message)
        {
            var senderId = Context.UserIdentifier;

            // Call the service to save the message
            await _conversationService.SendMessage(
                Convert.ToInt32(senderId),
                Convert.ToInt32(conversationId),
                message);

            // Broadcast to conversation group
            await Clients.Group(conversationId)
                .SendAsync("ReceiveMessage",
                    conversationId,
                    senderId,
                    message);
        }
    }
}
