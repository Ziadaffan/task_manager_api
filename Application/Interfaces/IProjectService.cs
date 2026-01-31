using Domaine.Classes;

namespace Application.Interfaces
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<List<Project>> GetAll();
        Task<List<Project>> GetAllProjectsByOrganisationId(Guid id);
    }
}
