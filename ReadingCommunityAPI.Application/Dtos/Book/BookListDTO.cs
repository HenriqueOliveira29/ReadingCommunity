namespace ReadingCommunityApi.Application.Dtos;

public class BookListDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime PublicationDate { get; set; }
    public string AuthorName { get; set; } = "";
}