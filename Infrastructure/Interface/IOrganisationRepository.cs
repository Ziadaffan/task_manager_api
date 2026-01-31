using Domaine.Classes;

namespace Infrastructure.Interface
{
    public interface IOrganisationRepository
    {
        Task<List<Organisation>> GetAll();
        Task<Organisation> GetById(Guid id);
        Task<Organisation> Add(Organisation organisation);
        Task<Organisation> Update(Organisation organisation);
        Task<bool> Delete(Guid id);
    }
}
