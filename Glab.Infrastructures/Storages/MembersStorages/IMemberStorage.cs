using GLAB.Domains.Models.Members;

namespace Glab.Infrastructures.Storages.MembersStorages;

public interface IMemberStorage
{
    Task<List<Member>> SelectMembers();

    Task InsertMember(Member member);

    Task UpdateMember(Member member);

    Task UpdateMemberStatus(string id);

    Task<bool> MemberExistsById(string id);
    Task<Member> SelectMemberByEmail(string email);
    Task<Member> SelectMemberByName(string name);


}