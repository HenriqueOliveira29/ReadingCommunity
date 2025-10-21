using ReadingCommunityApi.Core.Dtos;

namespace ReadingCommunityApi.Core.Dto;

public class AuthorDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<BookListDTO> books{ get; set; }
}