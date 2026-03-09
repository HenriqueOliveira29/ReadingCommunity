using Microsoft.AspNetCore.SignalR;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Infrastructure.Data;
using System.Security.Claims;

namespace ReadingCommunityApi.Web
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
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
            await _messageService.SaveMessageAsync(
                conversationId,
                senderId,
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
