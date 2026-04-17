using BuggyBackend.Models;
using BuggyBackend.Repositories;

namespace BuggyBackend.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAll();
        }

        public Member GetMemberById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid member ID");
            }
            return _memberRepository.GetById(id);
        }

        public Member GetMemberByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required");
            }
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
            {
                throw new ArgumentException("Member email is invalid");
            }
            return _memberRepository.GetByEmail(email);
        }

        public Member AddMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (string.IsNullOrWhiteSpace(member.Name))
            {
                throw new ArgumentException("Member name is required");
            }

            if (string.IsNullOrWhiteSpace(member.Email))
            {
                throw new ArgumentException("Member email is required");
            }
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(member.Email))
            {
                throw new ArgumentException("Member email is invalid");
            }

            var existingMember = _memberRepository.GetByEmail(member.Email);
            if (existingMember != null)
            {
                throw new InvalidOperationException("A member with this email already exists");
            }
            if (string.IsNullOrWhiteSpace(member.Name))
            {
                throw new ArgumentException("Member name is required");
            }
            if (string.IsNullOrWhiteSpace(member.Email))
            {
                throw new ArgumentException("Member email is required");
            }

            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(member.Email))
            {
                throw new ArgumentException("Member email is invalid");
            }

            return _memberRepository.Create(member);
        }

        public Member UpdateMember(int id, Member member)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid member ID");
            }

            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var existingMember = _memberRepository.GetById(id);
            if (existingMember == null)
            {
                throw new KeyNotFoundException($"Member with ID {id} not found");
            }
            var memberWithSameEmail = _memberRepository.GetByEmail(member.Email);
            if (memberWithSameEmail != null && memberWithSameEmail.Id != id)
            {
                throw new InvalidOperationException("A member with this email already exists");
            }

            return _memberRepository.Update(id, member);
        }

        public bool DeleteMember(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid member ID");
            }

            var member = _memberRepository.GetById(id);
            if (member == null)
            {
                return false;
            }

            if (member.BorrowedBookIds.Count > 0)
            {
                throw new InvalidOperationException("Cannot delete member with borrowed books");
            }

            return _memberRepository.Delete(id);
        }
    }
}
