using BuggyBackend.Models;
using BuggyBackend.Repositories;

namespace BuggyBackend.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ITransactionRepository _transactionRepository;
        public BookService(IBookRepository bookRepository, ITransactionRepository transactionRepository)
        {
            _bookRepository = bookRepository;
            _transactionRepository = transactionRepository;
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid book ID");
            }
            return _bookRepository.GetById(id);
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new List<Book>();
            }
            return _bookRepository.SearchByTitle(title);
        }

        public Book AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException("Book title is required");
            }

            if (string.IsNullOrWhiteSpace(book.Author))
            {
                throw new ArgumentException("Book author is required");
            }
            if (string.IsNullOrWhiteSpace(book.ISBN))
            {
                throw new ArgumentException("Book ISBN is required");
            }
            if (book.PublishedYear <= 0 || book.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year is invalid");
            }

            if (book.AvailableCopies < 0)
            {
                throw new ArgumentException("Available copies cannot be negative");
            }

            return _bookRepository.Create(book);
        }

        public Book UpdateBook(int id, Book book)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid book ID");
            }

            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var existingBook = _bookRepository.GetById(id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found");
            }

            return _bookRepository.Update(id, book);
        }


        public bool DeleteBook(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid book ID");
            }

            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return false;
            }

            var hasActiveLoans = _transactionRepository
                .GetActiveTransactions()
                .Any(t => t.BookId == id);

            if (hasActiveLoans)
            {
                throw new InvalidOperationException("Cannot delete a book with active loans");
            }

            return _bookRepository.Delete(id);
        }

        public List<Book> GetAvailableBooks()
        {
            var allBooks = _bookRepository.GetAll();
            return allBooks.Where(b => b.IsAvailable()).ToList();
        }
    }
}
