using BookstoreApi.Dtos;

namespace BookstoreApi.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetByIsbnAsync(string isbn);
        Task AddBookAsync(CreateBookDto newBookDto);
        Task UpdateBookAsync(string isbn, UpdateBookDto updateBookDto);
        Task DeleteBookAsync(string isbn);
    }
}
