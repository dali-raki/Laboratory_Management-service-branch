using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;


namespace Glab.App.Members
{
    public interface IMemberService
    {
        ValueTask<List<Member>> GetMembers();
        /*        ValueTask<Member> GetMemberById(string id);*/
        ValueTask<Member> GetMemberByEmail(string email);
        ValueTask<Result> SetMemberStatus(string id);
        ValueTask<Result> SetMember(Member member);
        ValueTask<Result> CreateMember(Member member);
        ValueTask<Member> GetMemberByName(string name);
    }
}