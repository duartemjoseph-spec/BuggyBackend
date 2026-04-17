using BuggyBackend.Models;

namespace BuggyBackend.Services
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member GetMemberById(int id);
        Member GetMemberByEmail(string email);
        Member AddMember(Member member);
        Member UpdateMember(int id, Member member);
        bool DeleteMember(int id);
    }
}
