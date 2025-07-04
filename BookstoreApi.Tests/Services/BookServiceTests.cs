using AutoMapper;
using BookstoreApi.Dtos;
using BookstoreApi.Exceptions;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Implementations;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookstoreApi.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BookService>>();

            _bookService = new BookService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetByIsbnAsync_BookNotFound_ReturnsNull()
        {
            // Arrange
            string isbn = "0000000000000";
            _mockRepo.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync((Book)null);
            _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns((BookDto)null);

            // Act
            var result = await _bookService.GetByIsbnAsync(isbn);

            // Assert
            Assert.Null(result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ExistingBook_ThrowsInvalidBookDataException()
        {
            // Arrange
            var newBookDto = new CreateBookDto { Isbn = "1234567890123", Title = "New Book" };
            _mockRepo.Setup(r => r.GetByIsbnAsync(newBookDto.Isbn))
                     .ReturnsAsync(new Book { Isbn = newBookDto.Isbn });

            // Act + Assert
            await Assert.ThrowsAsync<InvalidBookDataException>(() => _bookService.AddBookAsync(newBookDto));
        }

        [Fact]
        public async Task AddBookAsync_NewBook_CallsRepositoryAdd()
        {
            // Arrange
            var newBookDto = new CreateBookDto { Isbn = "1234567890123", Title = "New Book" };
            _mockRepo.Setup(r => r.GetByIsbnAsync(newBookDto.Isbn)).ReturnsAsync((Book)null);
            _mockMapper.Setup(m => m.Map<Book>(newBookDto)).Returns(new Book { Isbn = newBookDto.Isbn });

            // Act
            await _bookService.AddBookAsync(newBookDto);

            // Assert
            _mockRepo.Verify(r => r.AddBookAsync(It.Is<Book>(b => b.Isbn == newBookDto.Isbn)), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_BookNotFound_ThrowsBookNotFoundException()
        {
            // Arrange
            string isbn = "1234567890123";
            var updateDto = new UpdateBookDto { Title = "Updated Title" };
            _mockRepo.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync((Book)null);

            // Act + Assert
            await Assert.ThrowsAsync<BookNotFoundException>(() => _bookService.UpdateBookAsync(isbn, updateDto));
        }

        [Fact]
        public async Task UpdateBookAsync_ExistingBook_CallsRepositoryUpdate()
        {
            // Arrange
            string isbn = "1234567890123";
            var existingBook = new Book { Isbn = isbn };
            var updateDto = new UpdateBookDto { Title = "Updated Title" };
            var updatedBook = new Book { Isbn = isbn, Title = "Updated Title" };

            _mockRepo.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync(existingBook);
            _mockMapper.Setup(m => m.Map<Book>(updateDto)).Returns(updatedBook);

            // Act
            await _bookService.UpdateBookAsync(isbn, updateDto);

            // Assert
            _mockRepo.Verify(r => r.UpdateBookAsync(isbn, updatedBook), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_BookNotFound_ThrowsBookNotFoundException()
        {
            // Arrange
            string isbn = "1234567890123";
            _mockRepo.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync((Book)null);

            // Act + Assert
            await Assert.ThrowsAsync<BookNotFoundException>(() => _bookService.DeleteBookAsync(isbn));
        }

        [Fact]
        public async Task DeleteBookAsync_ExistingBook_CallsRepositoryDelete()
        {
            // Arrange
            string isbn = "1234567890123";
            var existingBook = new Book { Isbn = isbn };
            _mockRepo.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync(existingBook);

            // Act
            await _bookService.DeleteBookAsync(isbn);

            // Assert
            _mockRepo.Verify(r => r.DeleteBookAsync(isbn), Times.Once);
        }

    }
}
