namespace ReadingCommunityApi.Core.Models;

public class Author
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Biography { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public DateTime? DeathDate { get; private set; }
    public string Nationality { get; private set; }
    public string ProfileImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public ICollection<Book> Books { get; private set; } = new List<Book>();

    private Author() { } //EMpty constructure to the EF Core
    public Author(string name, string biography, DateTime? birthDate,
                  string nationality, string profileImageUrl)
    {
        Name = name;
        Biography = biography;
        BirthDate = birthDate;
        Nationality = nationality;
        ProfileImageUrl = profileImageUrl;
        CreatedAt = DateTime.UtcNow;
    }
    public void UpdateProfileImage(string profileImageUrl)
    {
        ProfileImageUrl = profileImageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsAlive => DeathDate == null;
    public int? Age
    {
        get
        {
            if (BirthDate == null) return null;
            var endDate = DeathDate ?? DateTime.UtcNow;
            var age = endDate.Year - BirthDate.Value.Year;
            if (endDate < BirthDate.Value.AddYears(age)) age--;
            return age;
        }
    }   
}