using Application.Response;
using Domaine.Classes;

namespace Application.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<List<User>> GetAll();
        Task<List<User>> GetByOrganisationId(Guid organisationId);
        Task<LogInResponse> LogIn(string username, string password);
    }
}
