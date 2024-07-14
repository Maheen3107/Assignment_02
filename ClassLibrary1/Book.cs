public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public bool IsAvailable { get; set; }

    // Constructor to initialize properties
    public Book()
    {
        Author = ""; // Initialize with empty string or appropriate default value
        Genre = "";  // Initialize with empty string or appropriate default value
        Title = "";
    }
}
