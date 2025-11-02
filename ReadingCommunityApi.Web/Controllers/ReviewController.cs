using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;

    }

    [HttpPost]
    public async Task<ActionResult<OperationResult>> Create([FromBody] ReviewCreateDTO reviewCreate)
    {
        var result = await _reviewService.AddReview(reviewCreate);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult>> Create(int id)
    {
        var result = await _reviewService.GetReviewsByBook(id);
        return StatusCode(result.StatusCode, result);
    }
}