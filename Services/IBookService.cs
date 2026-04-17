using BuggyBackend.Models;

namespace BuggyBackend.Services
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        List<Book> SearchBooksByTitle(string title);
        Book AddBook(Book book);
        Book UpdateBook(int id, Book book);
        bool DeleteBook(int id);
        List<Book> GetAvailableBooks();
    }
}
