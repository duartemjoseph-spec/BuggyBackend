using Microsoft.AspNetCore.Mvc;
using BuggyBackend.Models;
using BuggyBackend.Services;

namespace BuggyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpPost("borrow")]
public ActionResult<Transaction> BorrowBook([FromBody] BorrowRequest request)
{
    try
    {
        var transaction = _libraryService.BorrowBook(request.MemberId, request.BookId);
        return CreatedAtAction(
            nameof(GetMemberTransactions),
            new { memberId = transaction.MemberId },
            transaction
        );
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}

        [HttpPut("return/{transactionId}")]
        public ActionResult<Transaction> ReturnBook(int transactionId)
        {
            try
            {
                var transaction = _libraryService.ReturnBook(transactionId);
                return Ok(transaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("transactions/member/{memberId}")]
        public ActionResult<List<Transaction>> GetMemberTransactions(int memberId)
        {
            var transactions = _libraryService.GetMemberTransactions(memberId);
            return Ok(transactions);
        }

        [HttpGet("transactions/active")]
        public ActionResult<List<Transaction>> GetActiveTransactions()
        {
            var transactions = _libraryService.GetActiveTransactions();
            return Ok(transactions);
        }

        [HttpGet("overdue")]
        public ActionResult<List<Book>> GetOverdueBooks()
        {
            var books = _libraryService.GetOverdueBooks();
            return Ok(books);
        }
    }

    public class BorrowRequest
    {
        public int MemberId { get; set; }
        public int BookId { get; set; }
    }
}
