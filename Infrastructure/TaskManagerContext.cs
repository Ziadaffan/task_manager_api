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

            modelBuilder.Entity<Organisation>()
                .HasMany(o => o.Projects)
                .WithOne(p => p.Organisation)
                .HasForeignKey(p => p.OrganisationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organisation>()
                .HasMany(o => o.Users)
                .WithOne(u => u.Organisation)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.TaskProject)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AssignedTasks)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Domaine.Classes.Task> Tasks => Set<Domaine.Classes.Task>();
        public DbSet<Organisation> Organisations => Set<Organisation>();
    }
}
