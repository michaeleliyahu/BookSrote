using BookstoreApi.Dtos;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BookstoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<BookDto>> GetByIsbn(string isbn)
        {
            var book = await _bookService.GetByIsbnAsync(isbn);
            if (book == null)
                return NotFound(new { message = $"Book with ISBN {isbn} not found." });

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBookDto newBookDto)
        {
            await _bookService.AddBookAsync(newBookDto);
            return CreatedAtAction(nameof(GetByIsbn), new { isbn = newBookDto.Isbn }, newBookDto);
        }

        [HttpPut("{isbn}")]
        public async Task<ActionResult> Update(string isbn, [FromBody] UpdateBookDto updateBookDto)
        {
            await _bookService.UpdateBookAsync(isbn, updateBookDto);
            return NoContent();
        }

        [HttpDelete("{isbn}")]
        public async Task<ActionResult> Delete(string isbn)
        {
            await _bookService.DeleteBookAsync(isbn);
            return NoContent();
        }

        [HttpGet("report/download")]
        public async Task<IActionResult> DownloadHtmlReport()
        {
            var books = await _bookService.GetAllBooksAsync();
            var html = HtmlReportHelper.GenerateBooksReportHtml(books);

            var bytes = Encoding.UTF8.GetBytes(html);
            var fileName = "BooksReport.html";

            return File(bytes, "text/html", fileName);
        }

    }
}
