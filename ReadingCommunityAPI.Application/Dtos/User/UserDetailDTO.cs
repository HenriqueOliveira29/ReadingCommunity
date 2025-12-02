namespace ReadingCommunityApi.Application.Dtos;

public class UserDetailDTO
{
    public int Id {get; set; }
    public string UserName { get; set; }
    public string ProfileImageUrl { get; set; }
    public int NumberFollowers {get; set; }
    public int NumberFollowing {get; set; }
    public List<WishlistCollectionListDTO> WishLists {get; set;} = new List<WishlistCollectionListDTO>(); 

}