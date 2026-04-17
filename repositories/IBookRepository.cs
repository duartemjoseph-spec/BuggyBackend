using BuggyBackend.Models;

namespace BuggyBackend.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book GetById(int id);
        List<Book> SearchByTitle(string title);
        Book Create(Book book);
        Book Update(int id, Book book);
        bool Delete(int id);
        bool DecrementCopies(int id);
        bool IncrementCopies(int id);
    }
}
