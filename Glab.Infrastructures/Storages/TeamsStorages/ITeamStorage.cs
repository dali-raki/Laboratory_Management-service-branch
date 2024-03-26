using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;

namespace Glab.Infrastructures.Storages.TeamsStorages;

public interface ITeamStorage
{
    Task<List<Team>> SelectTeams();

    Task<List<Team?>> SelectTeamsByLaboratoryId(string LaboratoryId);

    Task<Team?> SelectTeamById(string TeamId);

    Task<Team?> SelectTeamByName(string TeamName);

    Task InsertTeam(Team team);

    Task UpdateTeam(Team team);

    Task<bool> ExistId(string TeamId);

    Task<bool> ExistName(string TeamName);

    Task UpdateTeamStatus(string TeamId, TeamStatus status);

    Task<bool> IsTeamInLaboratory(string TeamId, string LaboratoryId);

    Task<List<Member>> GetTeamMembers(string TeamId);
}