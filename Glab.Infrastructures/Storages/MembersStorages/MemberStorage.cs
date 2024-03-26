using System.Data;
using System.Data.SqlClient;
using GLAB.Domains.Models.Members;
using Microsoft.Extensions.Configuration;

namespace Glab.Infrastructures.Storages.MembersStorages;

public class MemberStorage : IMemberStorage
{
    private readonly string connectionString;

    public MemberStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");
    }

    public async Task UpdateMemberStatus(string id)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string updateCommandText = @"
        UPDATE dbo.Members
        SET Status = -1
        WHERE Id = @aMemberId";

        SqlCommand cmd = new SqlCommand(updateCommandText, connection);

        cmd.Parameters.AddWithValue("@aMemberId", id);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task InsertMember(Member member)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await insertRow(connection, member);
    }

    private static async Task insertRow(SqlConnection connection, Member member)
    {
        SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Members(MemberId, FirstName, LastName, Email, NIC, PhoneNumber, Image) " +
                                        "VALUES(@aMemberId, @aFirstName, @aLastName, @aEmail, @aNIC, @aPhoneNumber, @aImage)", connection);

        cmd.Parameters.AddWithValue("@aMemberId", member.MemberId);
        cmd.Parameters.AddWithValue("@aFirstName", member.FirstName);
        cmd.Parameters.AddWithValue("@aLastName", member.LastName);
        cmd.Parameters.AddWithValue("@aEmail", member.Email);
        cmd.Parameters.AddWithValue("@aNIC", member.NIC);
        cmd.Parameters.AddWithValue("@aPhoneNumber", member.PhoneNumber);
        cmd.Parameters.AddWithValue("@aLogo", member.Image);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Member>> SelectMembers()
    {
        List<Member> members = new List<Member>();
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new("SELECT * VMembers", connection);

        DataTable dt = new();
        SqlDataAdapter da = new(cmd);

        await connection.OpenAsync();
        da.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {
            Member member = getMemberFromDataRow(row);
            members.Add(member);
        }

        return members;
    }

    public async Task UpdateMember(Member member)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await updateRow(connection, member);
    }

    private static async Task updateRow(SqlConnection connection, Member member)
    {
        string updateCommandText = @"
        UPDATE dbo.Members
        SET FirstName = @aFirstName,
            LastName = @aLastName,
            NIC = @aNIC,
            PhoneNumber = @aPhoneNumber,
            Image = @aImage
        WHERE MemberId = @MemberId";

        SqlCommand cmd = new SqlCommand(updateCommandText, connection);

        cmd.Parameters.AddWithValue("@aFirstName", member.FirstName);
        cmd.Parameters.AddWithValue("@aLastName", member.LastName);
        cmd.Parameters.AddWithValue("@aNIC", member.NIC);
        cmd.Parameters.AddWithValue("@aPhoneNumber", member.PhoneNumber);
        cmd.Parameters.AddWithValue("@aImage", member.Image);
        cmd.Parameters.AddWithValue("@MemberId", member.MemberId);

        await cmd.ExecuteNonQueryAsync();
    }

    private static Member getMemberFromDataRow(DataRow row)
    {
        return new Member
        {
            MemberId = (string)row["MemberId"],
            FirstName = (string)row["FirstName"],
            LastName = (string)row["LastName"],
            Email = (string)row["Email"],
            NIC = (string)row["NIC"],
            PhoneNumber = (string)row["PhoneNumber"],
            Image = (byte[])row["Image"]
        };
    }

    public async Task<bool> MemberExistsById(string id)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) FROM VMembers WHERE MemberId = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        int count = (int)await command.ExecuteScalarAsync();

        return count > 0;
    }
    private const string selectMemberByEmailQuery = "SELECT COUNT(*) FROM VMembers WHERE email = @aemail";

    public async Task<Member> SelectMemberByEmail(string email)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectMemberByEmailQuery, connection);

        command.Parameters.AddWithValue("@aemail", email);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        return dt.Rows.Count == 0 ? null : getMemberFromDataRow(dt.Rows[0]);
    }
    private const string selectMemberByNameQuery = "SELECT COUNT(*) FROM VMembers WHERE name = @aname";

    public async Task<Member> SelectMemberByName(string name)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectMemberByEmailQuery, connection);

        command.Parameters.AddWithValue("@aname", name);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        return dt.Rows.Count == 0 ? null : getMemberFromDataRow(dt.Rows[0]);
    }

}