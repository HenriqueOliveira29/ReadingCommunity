namespace ReadingCommunityApi.Application.Dtos;

public class BookDetailDTO
{
    public int Id {get; set;}
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
}