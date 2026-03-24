using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task SendMessage(string conversationId, string message)
        {
            var senderId = Context.UserIdentifier;
            
            if (string.IsNullOrEmpty(senderId)) return;

            // 2. Use TryParse or ensure your Identity setup uses numeric IDs
            if (int.TryParse(senderId, out int sId) && int.TryParse(conversationId, out int cId))
            {
                await _conversationService.SendMessage(sId, cId, message);

                await Clients.Group(conversationId).SendAsync("ReceiveMessage", 
                    conversationId, senderId, message);
            }
        }
    }
}
