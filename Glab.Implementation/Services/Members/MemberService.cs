using System.Transactions;
using Glab.App.Members;
using Glab.Infrastructures.Storages.MembersStorages;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;

namespace GLAB.Implementation.Services.Members
{
    public class MemberService : IMemberService
    {
        private readonly IMemberStorage memberStorage;

        public MemberService(IMemberStorage memberStorage)
        {
            this.memberStorage = memberStorage;
        }

        public async ValueTask<Result> CreateMember(Member member)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    List<ErrorCode> errorList = await validateMemberForInsert(member);

                    if (errorList.Any())
                        return Result.Failure(errorList); 

                    await memberStorage.InsertMember(member);
                    scope.Complete();

                    return Result.Succes;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting member: {ex }");
                    return Result.Failure(new [] { "An error occurred while inserting the member." });
                }
            }
        }

        private async ValueTask<List<ErrorCode>> validateMemberForInsert(Member member)
        {
            List<ErrorCode> errors = new List<ErrorCode>();

            if (string.IsNullOrWhiteSpace(member.MemberId))
                errors.Add(MemberError.IdEmpty);

            if (string.IsNullOrWhiteSpace(member.FirstName))
                errors.Add(MemberError.FirstNameEmpty);

            if (string.IsNullOrWhiteSpace(member.LastName))
                errors.Add(MemberError.LastNameEmpty);

            if (string.IsNullOrWhiteSpace(member.Email))
                errors.Add(MemberError.EmailEmpty);

            if (string.IsNullOrWhiteSpace(member.NIC))
                errors.Add(MemberError.NICEmpty);

            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                errors.Add(MemberError.PhoneNumberEmpty);

            if (member.Image is null)
                errors.Add(MemberError.PhotoEmpty);

            if (string.IsNullOrWhiteSpace(member.MemberId))
            {
                bool memberExists = await memberStorage.MemberExistsById(member.MemberId);
                if (memberExists)
                    throw new Exception("Member with the same id already exists.");
            }
                return errors;
        }
            public async ValueTask<Result> SetMemberStatus(string id)
            {
                try
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                    List<ErrorCode> errors = new List<ErrorCode>();
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            errors.Add(MemberError.IdEmpty);
                            return Result.Failure(errors);
                        }

                        await memberStorage.UpdateMemberStatus(id);

                        scope.Complete();
                        return Result.Succes;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during member removal: {ex }");
                    return Result.Failure(new[] { "An error occurred during member removal." });
                }
            }

            public async ValueTask<Result> SetMember(Member member)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        List<ErrorCode> errorList = validateMemberForUpdate(member);
                        if (errorList.Any())
                            return Result.Failure(errorList.Select(e => e ).ToList());

                        await memberStorage.UpdateMember(member);
                        scope.Complete();
                        return Result.Succes;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting member: {ex }");
                        return Result.Failure(new[] { "An error occurred while setting the member." });
                    }
                }
            }

            private List<ErrorCode> validateMemberForUpdate(Member member)
            {
                List<ErrorCode> errors = new List<ErrorCode>();

                if (string.IsNullOrWhiteSpace(member.MemberId))
                    errors.Add(MemberError.IdEmpty );

/*                if (object.IsNullOrWhiteSpace(member.Status)
                    errors.Add(MemberError.StatusEmpty);*/

                if (string.IsNullOrWhiteSpace(member.FirstName))
                    errors.Add(MemberError.FirstNameEmpty);

                if (string.IsNullOrWhiteSpace(member.LastName))
                    errors.Add(MemberError.LastNameEmpty);

                if (string.IsNullOrWhiteSpace(member.Email))
                    errors.Add(MemberError.EmailEmpty);

                if (string.IsNullOrWhiteSpace(member.NIC))
                    errors.Add(MemberError.NICEmpty);

                if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                    errors.Add(MemberError.PhoneNumberEmpty);

                if (member.Image is null)
                    errors.Add(MemberError.PhotoEmpty);

                return errors;
            }


            public async ValueTask<List<Member>> GetMembers()
            {
                try
                {
                    List<Member> members = await memberStorage.SelectMembers();

                    if (members == null)
                    {
                        throw new Exception("Members list is null.");
                    }

                    return members;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting members: {ex }");
                    throw;
                }
            }


/*        public async ValueTask<Member> GetMemberById(string id)
        {

                return await memberStorage.SelectMemberById(id);
            

        }*/

        public async ValueTask<Member> GetMemberByName(string name)
            {
                try
                {
                    return await memberStorage.SelectMemberByName(name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting member by name: {ex }");
                    throw;
                }
            }

            public async ValueTask<Member> GetMemberByEmail(string email)
            {
                try
                {
                    return await memberStorage.SelectMemberByEmail(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting member by email: {ex }");
                    throw;
                }
            }

        }
    }