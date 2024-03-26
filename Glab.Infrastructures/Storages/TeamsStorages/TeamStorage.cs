using System.Data;
using System.Data.SqlClient;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.Extensions.Configuration;

namespace Glab.Infrastructures.Storages.TeamsStorages;

public class TeamStorage : ITeamStorage
{
    private readonly string connectionString;
    private const string selectTeamsQuery = "select * from dbo.Teams";
    private const string selectTeamByIdQuery = "select * from dbo.Teams where TeamId = @aTeamId";
    private const string selectTeamByNameQuery = "select * from dbo.Teams where TeamName = @aTeamName";
    private const string insertTeamQuery = "Insert into dbo.Teams(TeamId, Status, LaboratoryId, TeamName) VALUES(@aTeamId, @aStatus, @aLaboratoryId, @aTeamName)";
    private const string updateTeamQuery = "UPDATE dbo.Teams SET TeamName = @aTeamName, LaboratoryId = @aLaboratoryId, Status = @Status WHERE TeamId = @TeamId";
    private const string existIdQuery = "select * from dbo.Teams where TeamId = @aTeamId";
    private const string existNameQuery = "select * from dbo.Teams where TeamName = @aTeamName";
    private const string selectTeamsByLaboratoryQuery = "select * from dbo.Teams where LaboratoryId = @aLaboratoryId";
    private const string updateStatusQuery = "UPDATE dbo.Teams SET Status = @Status WHERE TeamId = @TeamId";
    private const string selectMembersQuery = "SELECT * FROM Members WHERE TeamId = @TeamId";

    public TeamStorage(IConfiguration configuration) =>
        connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");

    public async Task<bool> IsTeamInLaboratory(string teamId, string laboratoryId)
    {
        Team? team = await SelectTeamById(teamId);
        return team != null && team.LaboratoryId == laboratoryId;
    }

    public async Task<List<Member>> GetTeamMembers(string teamId)
    {
        await using var connection = new SqlConnection(connectionString);

        var cmd = new SqlCommand(selectMembersQuery, connection);
        cmd.Parameters.AddWithValue("@TeamId", teamId);

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        await connection.OpenAsync();

        da.Fill(dt);

        return (from DataRow row in dt.Rows select getMemberFromDataRow(row)).ToList();
    }

    private static Member getMemberFromDataRow(DataRow row)
    {
        return new Member
        {
            MemberId = row["MemberId"].ToString(),
            FirstName = row["FirstName"].ToString(),
            LastName = row["LastName"].ToString(),
            Email = row["Email"].ToString(),
            NIC = row["NIC"].ToString(),
            PhoneNumber = row["PhoneNumber"].ToString(),
            Image = row["Image"] as byte[],
            Status = (MemberStatus)(int)row["Status"]
        };
    }

    public async Task<List<Team>> SelectTeams()
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamsQuery, connection);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return (from DataRow row in ds.Rows select getTeamFromDataRow(row)).ToList();
    }

    public async Task<Team?> SelectTeamById(string teamId)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new(selectTeamByIdQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamId", teamId);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count == 0 ? null : getTeamFromDataRow(ds.Rows[0]);
    }

    public async Task<Team?> SelectTeamByName(string TeamName)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamByNameQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamName", TeamName);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count == 0 ? null : getTeamFromDataRow(ds.Rows[0]);
    }

    public async Task InsertTeam(Team team)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertTeamQuery, connection);

        cmd.Parameters.AddWithValue("@aTeamId", team.TeamId);
        cmd.Parameters.AddWithValue("@aStatus", team.Status);
        cmd.Parameters.AddWithValue("@aLaboratoryId", team.LaboratoryId);
        cmd.Parameters.AddWithValue("@aTeamName", team.TeamName);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateTeam(Team team)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(updateTeamQuery, connection);

        cmd.Parameters.AddWithValue("@aTeamName", team.TeamName);
        cmd.Parameters.AddWithValue("@aStatus", team.Status);
        cmd.Parameters.AddWithValue("@aLaboratoryId", team.LaboratoryId);
        cmd.Parameters.AddWithValue("@aTeamId", team.TeamId);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static Team getTeamFromDataRow(DataRow row)
    {
        return new()
        {
            TeamName = (string)row["TeamName"],
            TeamId = (string)row["TeamId"],
            LaboratoryId = (string)row["LaboratoryId"],
            Status = (TeamStatus)(int)row["Status"]
        };
    }

    public async Task<bool> ExistId(string TeamId)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(existIdQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamId", TeamId);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count != 0;
    }

    public async Task<bool> ExistName(string TeamName)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(existNameQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamName", TeamName);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count != 0;
    }

    public async Task<List<Team?>> SelectTeamsByLaboratoryId(string LaboratoryId)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamsByLaboratoryQuery, connection);
        cmd.Parameters.AddWithValue("@aLaboratoryId", LaboratoryId);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return (from DataRow row in ds.Rows select getTeamFromDataRow(row)).ToList();
    }

    public async Task UpdateTeamStatus(string TeamId, TeamStatus status)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(updateStatusQuery, connection);

        cmd.Parameters.AddWithValue("@Status", (int)status);
        cmd.Parameters.AddWithValue("@TeamId", TeamId);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}