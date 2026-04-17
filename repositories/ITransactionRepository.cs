using BuggyBackend.Models;

namespace BuggyBackend.Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAll();
        Transaction GetById(int id);
        List<Transaction> GetByMemberId(int memberId);
        List<Transaction> GetByBookId(int bookId);
        List<Transaction> GetActiveTransactions();
        Transaction Create(Transaction transaction);
        Transaction Update(int id, Transaction transaction);
        bool Delete(int id);
    }
}
