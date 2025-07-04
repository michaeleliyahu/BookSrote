using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using System.Xml.Linq;

namespace BookstoreApi.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly string _xmlPath;

        public BookRepository(IConfiguration configuration)
        {
            _xmlPath = configuration["XmlDataPath"]
                       ?? throw new Exception("XmlDataPath is not configured.");
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var doc = await XmlHelper.LoadDocumentAsync(_xmlPath);

            return doc.Root?
                .Elements("book")
                .Select(x => new Book
                {
                    Isbn = x.Element("isbn")?.Value ?? string.Empty,
                    Title = x.Element("title")?.Value ?? string.Empty,
                    Authors = x.Elements("author").Select(a => a.Value).ToList(),
                    Category = x.Attribute("category")?.Value ?? string.Empty,
                    Year = int.TryParse(x.Element("year")?.Value, out var y) ? y : 0,
                    Price = decimal.TryParse(x.Element("price")?.Value, out var p) ? p : 0
                }).ToList() ?? new List<Book>();
        }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            var books = await GetAllBooksAsync();
            return books.FirstOrDefault(b => b.Isbn == isbn);
        }

        public async Task AddBookAsync(Book book)
        {
            var doc = await XmlHelper.LoadDocumentAsync(_xmlPath);

            var bookElement = new XElement("book",
                new XAttribute("category", book.Category),
                new XElement("isbn", book.Isbn),
                new XElement("title", book.Title),
                book.Authors.Select(a => new XElement("author", a)),
                new XElement("year", book.Year),
                new XElement("price", book.Price)
            );

            doc.Root?.Add(bookElement);
            await XmlHelper.SaveDocumentAsync(doc, _xmlPath);
        }

        public async Task UpdateBookAsync(string isbn, Book updatedBook)
        {
            var doc = await XmlHelper.LoadDocumentAsync(_xmlPath);

            var bookElement = doc.Root?
                .Elements("book")
                .FirstOrDefault(x => x.Element("isbn")?.Value == isbn);

            if (bookElement == null)
                throw new KeyNotFoundException($"Book with ISBN {isbn} not found.");

            // עדכון התכונות עם פונקציות עזר
            bookElement.SetAttributeValue("category", updatedBook.Category);

            bookElement.SetElementIfNotNull("title", updatedBook.Title);
            bookElement.SetElementsFromListIfNotNull("author", updatedBook.Authors);
            bookElement.SetElementIfNotNull("year", updatedBook.Year);
            bookElement.SetElementIfNotNull("price", updatedBook.Price);

            await XmlHelper.SaveDocumentAsync(doc, _xmlPath);
        }

        public async Task DeleteBookAsync(string isbn)
        {
            var doc = await XmlHelper.LoadDocumentAsync(_xmlPath);

            var bookElement = doc.Root?
                .Elements("book")
                .FirstOrDefault(x => x.Element("isbn")?.Value == isbn);

            bookElement?.Remove();

            await XmlHelper.SaveDocumentAsync(doc, _xmlPath);
        }
    }
}
