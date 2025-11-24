namespace ReadingCommunityApi.Application.Dtos;

public class BookCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; } 
    public string Description { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public int NumberOfPages { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
}