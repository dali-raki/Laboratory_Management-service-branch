using System.Data;
using System.Data.SqlClient;
using Glab.Domains.Models.Laboratories;
using GLAB.Domains.Models.Laboratories;
using Microsoft.Extensions.Configuration;

namespace Glab.Infrastructures.Storages.LaboratoriesStorages;

public class LaboratoryStorage : ILaboratoryStorage
{
    private readonly string connectionString;

    public LaboratoryStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane") ?? string.Empty;
    }

    private const string updateCommandText = "UPDATE dbo.LABORATORY SET Status = -1 WHERE Id = @aLaboratoryId";

    public async Task UpdateLaboratoryStatus(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(updateCommandText, connection);

        cmd.Parameters.AddWithValue("@aLaboratoryId", id);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Laboratory?>> SelectLaboratories()
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new("select * from VLABORATORY", connection);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return (from DataRow row in ds.Rows select getLaboratoireFromDataRow(row)).ToList();
    }

    private const string updateLaboratoryCommand = @"
        UPDATE dbo.LABORATORY
        SET Name = @aName,
            Adress = @aAdresse,
            Acronyme = @aAcronyme,
            PhoneNumber = @aPhoneNumber,
            Logo = @aLogo,
            WebSite = @aWebSite
        WHERE Id = @aLaboratoryId";

    public async Task UpdateLaboratory(Laboratory laboratory)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(updateLaboratoryCommand, connection);

        cmd.Parameters.AddWithValue("@aName", laboratory.Name);
        cmd.Parameters.AddWithValue("@aAdresse", laboratory.Adresse);
        cmd.Parameters.AddWithValue("@aAcronyme", laboratory.Acronyme);
        cmd.Parameters.AddWithValue("@aPhoneNumber", laboratory.PhoneNumber);
        cmd.Parameters.AddWithValue("@aLogo", laboratory.Logo);
        cmd.Parameters.AddWithValue("@aWebSite", laboratory.WebSite);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private const string insertLaboratoryCommand =
        "Insert INTO dbo.LABORATORY (LaboratoryId,Name, Adress,University, Faculty, Departement,Acronyme,PhoneNumber,Email,AgrementNumber,CreationDate,Logo,WebSite) " +
        "VALUES(@aLaboratoryId,@aName, @aAdress,@aUniversity,@aFaculty, @aDepartment,@aAcronyme,@aPhoneNumber,@aEmail,@aAgrementNumber,@aCreationDate,@aLogo,@aWebSite)";

    public async Task InsertLaboratory(Laboratory laboratory)
    {
        await using SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertLaboratoryCommand, connection);

        cmd.Parameters.AddWithValue("@aLaboratoryId", laboratory.LaboratoryId);
        cmd.Parameters.AddWithValue("@aName", laboratory.Name);
        cmd.Parameters.AddWithValue("@aAdress", laboratory.Adresse);
        cmd.Parameters.AddWithValue("@aUniversity", laboratory.University);
        cmd.Parameters.AddWithValue("@aDepartment", laboratory.Departement);
        cmd.Parameters.AddWithValue("@aAcronyme", laboratory.Acronyme);
        cmd.Parameters.AddWithValue("@aPhoneNumber", laboratory.PhoneNumber);
        cmd.Parameters.AddWithValue("@aEmail", laboratory.Email);
        cmd.Parameters.AddWithValue("@aFaculty", laboratory.Faculty);
        cmd.Parameters.AddWithValue("@aAgrementNumber", laboratory.NumAgrement);
        cmd.Parameters.AddWithValue("@aCreationDate", laboratory.CreationDate);
        cmd.Parameters.AddWithValue("@aLogo", laboratory.Logo);
        cmd.Parameters.AddWithValue("@aWebSite", laboratory.WebSite);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static Laboratory? getLaboratoireFromDataRow(DataRow row)
    {
        return new()
        {
            LaboratoryId = (string)row["LaboratoryId"],
            Name = (string)row["Name"],
            Adresse = (string)row["Adress"],
            University = (string)row["University"],
            Faculty = (string)row["Faculty"],
            Departement = (string)row["Departement"],
            Acronyme = (string)row["Acronyme"],
            PhoneNumber = (string)row["PhoneNumber"],
            Email = (string)row["Email"],
            NumAgrement = (string)row["AgrementNumber"],
            CreationDate = (DateTime)row["CreationDate"],
            Logo = (byte[])row["Logo"],
            WebSite = (string)row["WebSite"],
        };
    }

    private const string selectLaboratoryCountByAcronymeQuery = "SELECT COUNT(*) FROM VLABORATORY WHERE Acronyme = @aAcronyme";

    public async Task<bool> LaboratoryExistsByAcronyme(string acronyme)
    {
        await using var connection = new SqlConnection(connectionString);
        connection.Open();

        SqlCommand command = new SqlCommand(selectLaboratoryCountByAcronymeQuery, connection);
        command.Parameters.AddWithValue("@aAcronyme", acronyme);

        int count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    private const string selectLaboratoryCountByIdQuery = "SELECT COUNT(*) FROM LABORATORY WHERE LaboratoryId = @Id";

    public async Task<bool> LaboratoryExistsById(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand command = new SqlCommand(selectLaboratoryCountByIdQuery, connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();

        int count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    private const string selectLaboratoryStatusByIdQuery = "SELECT Status FROM VLABORATORY WHERE Id = @aId";

    public async Task<LaboratoryStatus> GetLaboratoryStatus(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryStatusByIdQuery, connection);
        command.Parameters.AddWithValue("@aId", id);
        connection.Open();

        return (LaboratoryStatus)(await command.ExecuteScalarAsync() ?? LaboratoryStatus.Deleted);
    }

    private const string selectLaboratoryByIdQuery = "SELECT * FROM VLABORATORY WHERE Id = @aId";

    public async Task<Laboratory?> SelectLaboratoryById(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryByIdQuery, connection);

        command.Parameters.AddWithValue("@aId", id);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        return dt.Rows.Count == 0 ? null : getLaboratoireFromDataRow(dt.Rows[0]);
    }
    private const string selectLaboratoryByNameQuery = "SELECT COUNT(*) FROM VLABORATORY WHERE name = @name";

    public async Task<Laboratory?> SelectLaboratoryByName(string name)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryByNameQuery, connection);

        command.Parameters.AddWithValue("@aname", name);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        return dt.Rows.Count == 0 ? null : getLaboratoireFromDataRow(dt.Rows[0]);
    }
    public async Task<bool> LaboratoryExistsByName(string name)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM VLABORATORY WHERE Name = @aName";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@aName", name);

                int count = (int)command.ExecuteScalar();

                return count > 0; ;
            }
        }

    }



}
