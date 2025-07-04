using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetByIsbnAsync(string isbn);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(string isbn, Book updatedBook);
        Task DeleteBookAsync(string isbn);
    }
}
