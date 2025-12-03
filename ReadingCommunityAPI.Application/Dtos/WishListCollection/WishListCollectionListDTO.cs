namespace ReadingCommunityApi.Application.Dtos;

public class WishlistCollectionListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public int NumberOfItems {get; set;}
}