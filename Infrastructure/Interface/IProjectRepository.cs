using Domaine.Classes;

namespace Infrastructure.Interface
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<List<Project>> GetAll();
        Task<List<Project>> GetAllProjectsByOrganisationId(Guid id);
    }
}
