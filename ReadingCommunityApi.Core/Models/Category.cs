namespace ReadingCommunityApi.Core.Models;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ICollection<Book> Books { get; private set; } = new List<Book>();

    private Category() { }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }
}