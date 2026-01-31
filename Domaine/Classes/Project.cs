namespace Domaine.Classes
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? OrganisationId { get; set; }


        public Project(string name, string description,User user, Guid orgId)
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
