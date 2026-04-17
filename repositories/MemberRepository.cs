using BuggyBackend.Models;

namespace BuggyBackend.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private List<Member> _members;
        private int _nextId;

        public MemberRepository()
        {
            _members = new List<Member>();
            _nextId = 1;
            InitializeData();
        }

        private void InitializeData()
        {
            _members = new List<Member>
            {
                new Member { Id = 1, Name = "Isaiah Ferguson", Email = "ifergusonlll@sjcoe.net", MembershipDate = DateTime.Now.AddYears(-2) },
                new Member { Id = 2, Name = "Jacob Dekok", Email = "jdekok@sjcoe.net", MembershipDate = DateTime.Now.AddYears(-1) },
                new Member { Id = 3, Name = "Ken Martinez", Email = "jmartenezlopez@sjcoe.net", MembershipDate = DateTime.Now.AddMonths(-6) }
            };
            _nextId = 4;
        }

        public List<Member> GetAll()
        {
            return _members;
        }

        public Member GetById(int id)
        {
            return _members.FirstOrDefault(m => m.Id == id);
        }

        public Member GetByEmail(string email)
        {
            return _members.FirstOrDefault(m => m.Email == email);
        }

        public Member Create(Member member)
        {
            member.Id = _nextId++;
            member.MembershipDate = DateTime.Now;
            _members.Add(member);
            return member;
        }

        public Member Update(int id, Member member)
        {
            var existingMember = GetById(id);
            if (existingMember != null)
            {
                existingMember.Name = member.Name;
                existingMember.Email = member.Email;
                existingMember.BorrowedBookIds = member.BorrowedBookIds ?? new List<int>();

                return existingMember;
            }
            return null;
        }

        public bool Delete(int id)
        {
            var member = GetById(id);
            if (member != null)
            {
                _members.Remove(member);
                return true;
            }
            return false;
        }

        public bool AddBorrowedBook(int memberId, int bookId)
        {
            var member = GetById(memberId);
            if (member != null)
            {
                member.BorrowedBookIds.Add(bookId);
                return true;
            }
            if (!member.BorrowedBookIds.Contains(bookId))
            {
                member.BorrowedBookIds.Add(bookId);
            }

            return false;
        }

        public bool RemoveBorrowedBook(int memberId, int bookId)
        {
            var member = GetById(memberId);
            if (member != null)
            {
                member.BorrowedBookIds.Remove(bookId);

            }
            return false;
        }
    }
}
