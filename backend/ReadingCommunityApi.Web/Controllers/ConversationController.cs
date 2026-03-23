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

    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out userId))
            return false;
        return true;
    }

    [HttpGet]
    public async Task<ActionResult> GetUserConversations()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid user claim" });

        var result = await _conversationService.GetUserConversations(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("create/{otherUserId}")]
    public async Task<ActionResult> CreateConversation(int otherUserId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid user claim" });
        var result = await _conversationService.CreateConversation(userId, otherUserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{conversationId}/messages")]
    public async Task<ActionResult> SendMessage(int conversationId, [FromBody] SendMessageRequest request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid user claim" });

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