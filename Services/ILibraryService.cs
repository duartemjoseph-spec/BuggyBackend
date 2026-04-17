using BuggyBackend.Models;

namespace BuggyBackend.Services
{
    public interface ILibraryService
    {
        Transaction BorrowBook(int memberId, int bookId);
        Transaction ReturnBook(int transactionId);
        List<Transaction> GetMemberTransactions(int memberId);
        List<Transaction> GetActiveTransactions();
        List<Book> GetOverdueBooks();
    }
}
