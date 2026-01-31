using Domaine.Classes;

namespace Application.Interfaces
{
    public interface IOrganisationService
    {
        Task<List<Organisation>> GetAll();
        Task<Organisation> GetById(Guid id);
        Task<Organisation> Add(Organisation organisation);
        Task<Organisation> Update(Organisation organisation);
        Task<bool> Delete(Guid id);
    }
}
