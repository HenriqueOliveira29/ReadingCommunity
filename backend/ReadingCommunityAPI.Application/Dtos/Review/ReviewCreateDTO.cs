namespace ReadingCommunityApi.Application.Dtos;

public class ReviewCreateDTO
{
    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime DatePosted { get; set; }
    
    public int BookId { get; set; }
}