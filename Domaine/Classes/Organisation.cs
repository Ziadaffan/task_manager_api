namespace Domaine.Classes
{
    public class Organisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public DateTime CreatedAt { get; set; }

        public Organisation(string name, string? description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public Organisation()
        {
            CreatedAt = DateTime.UtcNow;
            Projects = new List<Project>();
            Users = new List<User>();
        }

        public ICollection<Project> Projects { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
