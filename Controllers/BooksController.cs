using Microsoft.AspNetCore.Mvc;
using BuggyBackend.Models;
using BuggyBackend.Services;

namespace BuggyBackend.Controllers
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
        public ActionResult<List<Book>> GetAll()
        {
            var books = _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);
                if (book == null)
                {
                    return NotFound($"Book with ID {id} not found");
                }
                return Ok(book);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<List<Book>> Search([FromQuery] string title)
        {
            var books = _bookService.SearchBooksByTitle(title);
            return Ok(books);
        }

        [HttpGet("available")]
        public ActionResult<List<Book>> GetAvailable()
        {
            var books = _bookService.GetAvailableBooks();
            return Ok(books);
        }

        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            try
            {
                var createdBook = _bookService.AddBook(book);
                return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Book> Update(int id, [FromBody] Book book)
        {
            try
            {
                var updatedBook = _bookService.UpdateBook(id, book);
                return Ok(updatedBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _bookService.DeleteBook(id);
                if (!result)
                {
                    return NotFound($"Book with ID {id} not found");
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
