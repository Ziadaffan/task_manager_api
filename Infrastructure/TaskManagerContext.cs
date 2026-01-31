using Domaine.Classes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Domaine.Classes.Task> Tasks => Set<Domaine.Classes.Task>();
        public DbSet<Organisation> Organisations => Set<Organisation>();
    }
}
