namespace ReadingCommunityApi.Application.Dtos;

public class ReviewDetailDTO
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public int BookId { get; set; }

    public string BookName { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; }
}