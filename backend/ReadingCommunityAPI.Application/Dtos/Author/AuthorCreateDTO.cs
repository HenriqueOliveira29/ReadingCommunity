namespace ReadingCommunityApi.Application.Dtos;

public class AuthorCreateDTO
{
    public string Name { get; set; } = "";
    public string Biography { get; set; } = "";
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string Nationality { get; set; } = "";
    public string ProfileImageUrl { get; set; } = "";

}