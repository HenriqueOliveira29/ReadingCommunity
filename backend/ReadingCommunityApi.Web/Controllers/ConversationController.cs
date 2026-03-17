using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet]
    public async Task<ActionResult> GetUserConversations()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _conversationService.GetUserConversations(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("create/{otherUserId}")]
    public async Task<ActionResult> CreateConversation(int otherUserId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _conversationService.CreateConversation(userId, otherUserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{conversationId}/messages")]
    public async Task<ActionResult> SendMessage(int conversationId, [FromBody] SendMessageRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _conversationService.SendMessage(userId, conversationId, request.Message);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{conversationId}/messages")]
    public async Task<ActionResult> GetConversationMessages(int conversationId)
    {
        var conversation = await _conversationService.GetConversation(conversationId);
        if (conversation == null)
        {
            return NotFound();
        }
        return Ok(conversation.Messages.OrderBy(m => m.SentAt));
    }
}

public class SendMessageRequest
{
    public string Message { get; set; }
}