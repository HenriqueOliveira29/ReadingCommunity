namespace ReadingCommunityApi.Core.Dtos;

public class BookCreateDto
{
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; } 
    public string Description { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
}