using BuggyBackend.Models;
using BuggyBackend.Repositories;

namespace BuggyBackend.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ITransactionRepository _transactionRepository;

        public LibraryService(
            IBookRepository bookRepository,
            IMemberRepository memberRepository,
            ITransactionRepository transactionRepository)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _transactionRepository = transactionRepository;
        }

        public Transaction BorrowBook(int memberId, int bookId)
        {
            var member = _memberRepository.GetById(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException("Member not found");
            }

            var book = _bookRepository.GetById(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            if (!member.CanBorrowMore())
            {
                throw new InvalidOperationException("Member has reached borrowing limit");
            }

            if (!book.IsAvailable())
            {
                throw new InvalidOperationException("Book is not available");
            }
            if (member.BorrowedBookIds.Contains(bookId))
            {
                throw new InvalidOperationException("Member already borrowed this book");
            }

            _bookRepository.DecrementCopies(bookId);
            _memberRepository.AddBorrowedBook(memberId, bookId);

            var transaction = new Transaction
            {
                MemberId = memberId,
                BookId = bookId
            };

            return _transactionRepository.Create(transaction);
        }

        public Transaction ReturnBook(int transactionId)
        {
            var transaction = _transactionRepository.GetById(transactionId);
            if (transaction == null)
            {
                throw new KeyNotFoundException("Transaction not found");
            }

            if (transaction.IsReturned())
            {
                throw new InvalidOperationException("Book already returned");
            }

            transaction.ReturnDate = DateTime.Now;
            _transactionRepository.Update(transactionId, transaction);

            _bookRepository.IncrementCopies(transaction.BookId);
            _memberRepository.RemoveBorrowedBook(transaction.MemberId, transaction.BookId);

            return transaction;
        }

        public List<Transaction> GetMemberTransactions(int memberId)
        {
            return _transactionRepository.GetByMemberId(memberId);
        }

        public List<Transaction> GetActiveTransactions()
        {
            return _transactionRepository.GetActiveTransactions();
        }

        public List<Book> GetOverdueBooks()
        {
            var activeTransactions = _transactionRepository.GetActiveTransactions();
            var overdueDate = DateTime.Now.AddDays(-14);

            var overdueBookIds = activeTransactions
                .Where(t => t.BorrowDate < overdueDate)
                .Select(t => t.BookId)
                .ToList();

            var allBooks = _bookRepository.GetAll();
            return allBooks.Where(b => overdueBookIds.Contains(b.Id)).ToList();
        }
    }
}
