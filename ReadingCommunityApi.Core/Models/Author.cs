namespace ReadingCommunityApi.Core.Models;

public class Author
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<Book> Books { get; private set; } = new List<Book>();


    private Author() { } //EMpty constructure to the EF Core
    public Author(string name)
    {
        this.Name = name;
    }

    public void AddBook(Book book)
    {
        this.Books.Add(book);
    }
}