using Glab.Domains.Models.Laboratories;
using GLAB.Domains.Shared;


namespace Glab.App.Laboratoires
{
    public interface ILabService
    {
        Task<List<Laboratory>> GetLaboratories();

        Task<Laboratory> GetLaboratoryById(string id);
        Task<Laboratory> GetLaboratoryByName(string name);

        Task<Result> SetLaboratoryStatus(string id);

        Task<Result> SetLaboratory(Laboratory laboratory);

        Task<Result> CreateLaboratory(Laboratory laboratory);
    }

}