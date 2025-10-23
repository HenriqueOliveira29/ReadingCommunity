using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Dto;

public class AuthorDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<BookListDTO> books{ get; set; }
}