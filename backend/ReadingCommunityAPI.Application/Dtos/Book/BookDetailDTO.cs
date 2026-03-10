namespace ReadingCommunityApi.Application.Dtos;

public class BookDetailDTO
{
    public int Id {get; set;}
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
    public int NumberOfPages { get; set; }
    public string Dimensions {get; set; } = string.Empty;
    public List<ReviewDetailDTO> Reviews { get; set; } = new List<ReviewDetailDTO>();
    public List<string> Images { get; set; } = new List<string>();
    public List<string> Categories { get; set; } = new List<string>();
}