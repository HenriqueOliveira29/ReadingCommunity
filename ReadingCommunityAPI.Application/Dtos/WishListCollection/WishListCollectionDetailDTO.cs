namespace ReadingCommunityApi.Application.Dtos;

public class WishlistCollectionDetailDTO
{
    public int Id { get; set; }
    public string Name {get;set;}
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public bool IsDefault { get; set; }
    public List<WishListItemListDTO> Items {get; set; } = new List<WishListItemListDTO>();

}