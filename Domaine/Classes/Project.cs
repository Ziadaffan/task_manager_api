namespace Domaine.Classes
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? OrganisationId { get; set; }
        public Organisation? Organisation { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();


        public Project(string name, string? description,User user, Guid orgId)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            Owner = user;
            OrganisationId = orgId;
        }
        public Project()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
