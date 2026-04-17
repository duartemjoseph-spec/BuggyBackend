using BuggyBackend.Models;

namespace BuggyBackend.Repositories
{
    public interface IMemberRepository
    {
        List<Member> GetAll();
        Member GetById(int id);
        Member GetByEmail(string email);
        Member Create(Member member);
        Member Update(int id, Member member);
        bool Delete(int id);
        bool AddBorrowedBook(int memberId, int bookId);
        bool RemoveBorrowedBook(int memberId, int bookId);
    }
}
