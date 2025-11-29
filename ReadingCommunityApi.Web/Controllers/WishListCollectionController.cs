using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WishlistCollectionController : ControllerBase
{
    private readonly IWishListCollectionService _wishListCollectionService;

    public WishlistCollectionController(IWishListCollectionService wishListCollectionService)
    {
        _wishListCollectionService = wishListCollectionService;

    }

    [HttpGet]
    public async Task<ActionResult<OperationResult>> GetWishListCollection()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _wishListCollectionService.GetWishListCollectionByUserId(userId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("{id}")]
    public async  Task<ActionResult<OperationResult>> GetWishListCollectionById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _wishListCollectionService.GetWishListCollectionById(id, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<OperationResult>> CreateWishListCollection(WishlistCollectionCreateDTO dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _wishListCollectionService.CreateWishListCollection(dto, userId);
        return StatusCode(result.StatusCode, result);   
    }
}