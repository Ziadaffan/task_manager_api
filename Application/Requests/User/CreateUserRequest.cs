using Domaine.Enum;

namespace csharp_api.Requests.User
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
