using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Dtos;

public class WishlistCollectionCreateDTO
{
    public string Name {get; set;}
    public string Description {get; set;}
    public bool isPublic {get; set;}
    public bool isDefault {get; set;}
}