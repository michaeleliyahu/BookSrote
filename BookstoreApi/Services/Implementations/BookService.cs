using AutoMapper;
using BookstoreApi.Dtos;
using BookstoreApi.Exceptions;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;

namespace BookstoreApi.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository repository, IMapper mapper, ILogger<BookService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var books = await _repository.GetAllBooksAsync();
            return books.Select(_mapper.Map<BookDto>).ToList();
        }

        public async Task<BookDto?> GetByIsbnAsync(string isbn)
        {
            var book = await _repository.GetByIsbnAsync(isbn);
            if (book == null)
            {
                _logger.LogWarning("Book with ISBN {Isbn} not found.", isbn);
                return null;
            }
            return _mapper.Map<BookDto>(book);
        }

        public async Task AddBookAsync(CreateBookDto newBookDto)
        {
            var existingBook = await _repository.GetByIsbnAsync(newBookDto.Isbn);
            if (existingBook != null)
            {
                _logger.LogWarning("Add failed: Book with ISBN {Isbn} already exists.", newBookDto.Isbn);
                throw new InvalidBookDataException($"Book with ISBN {newBookDto.Isbn} already exists.");
            }

            var book = _mapper.Map<Book>(newBookDto);
            await _repository.AddBookAsync(book);
        }

        public async Task UpdateBookAsync(string isbn, UpdateBookDto updateBookDto)
        {
            var existingBook = await _repository.GetByIsbnAsync(isbn);
            if (existingBook == null)
            {
                _logger.LogWarning("Update failed: Book with ISBN {Isbn} not found.", isbn);
                throw new BookNotFoundException(isbn);
            }

            var updatedBook = _mapper.Map<Book>(updateBookDto);
            updatedBook.Isbn = isbn;
            await _repository.UpdateBookAsync(isbn, updatedBook);
        }

        public async Task DeleteBookAsync(string isbn)
        {
            var existingBook = await _repository.GetByIsbnAsync(isbn);
            if (existingBook == null)
            {
                _logger.LogWarning("Delete failed: Book with ISBN {Isbn} not found.", isbn);
                throw new BookNotFoundException(isbn);
            }

            await _repository.DeleteBookAsync(isbn);
        }

    }
}
