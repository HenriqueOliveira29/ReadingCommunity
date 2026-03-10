namespace ReadingCommunityApi.Application.Dtos;

public class WishListItemCreateDTO
{
    public int BookId {get; set;}
    public int CollectionId {get; set;}
    public int Priority {get; set;}
}