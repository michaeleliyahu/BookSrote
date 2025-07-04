namespace BookstoreApi.Models
{
    public class Book
    {
        public string Isbn { get; set; } = null!;  // מזהה ייחודי

        public string Title { get; set; } = null!;

        public List<string> Authors { get; set; } = new List<string>();

        public string Category { get; set; } = null!;

        public int Year { get; set; }

        public decimal Price { get; set; }
    }
}