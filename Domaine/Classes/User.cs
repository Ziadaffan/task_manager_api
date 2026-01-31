using Domaine.Enum;

namespace Domaine.Classes
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username
        {
            get;
            set;
        }
        public string Email
        {
            get; set;

        }
        public string PasswordHash { get; set; }
        public Role UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public Organisation? Organisation { get; set; }

        public User()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public User(string username, string email, string passwordHash, Organisation? organisation, Role role = Role.User)
        {
            Id = new Guid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            UserRole = role;
            CreatedAt = DateTime.UtcNow;
            Organisation = organisation;
        }
    }
}
