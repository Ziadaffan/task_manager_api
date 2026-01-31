using Domaine.Classes;

namespace Infrastructure.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAll();
        Task<List<User>> GetByOrganisationId(Guid organisationId);
        Task<User> GetByUsername(string username);
    }
}
