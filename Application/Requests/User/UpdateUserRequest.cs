using Domaine.Enum;

namespace csharp_api.Requests.User
{
    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
