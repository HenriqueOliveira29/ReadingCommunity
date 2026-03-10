namespace ReadingCommunityApi.Application.Dtos;

public class WishListItemListDTO
{
    public int Id { get; set; }
    public BookListDTO Book {get; set;}
    public DateTime AddedAt {get;set;}
    public int Priority {get; set;}
    public bool IsPurchased  {get;set;}
}