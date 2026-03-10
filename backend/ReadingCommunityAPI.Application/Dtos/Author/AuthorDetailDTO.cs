using ReadingCommunityApi.Application.Dtos;

namespace ReadingCommunityApi.Application.Dto;

public class AuthorDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }

    public int Age {get; set;}
    public bool isAlive {get; set;}
    public string Nationality { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<BookListDTO> books{ get; set; }
}