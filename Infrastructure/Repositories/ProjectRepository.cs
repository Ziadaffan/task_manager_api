
using Domaine.Classes;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagerContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(TaskManagerContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Project>> GetAll()
        {
            return await _context.Projects.Include(p => p.Owner).ToListAsync();
        }

        public async Task<Project> GetById(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                throw new KeyNotFoundException("User not found.");

            return project;
        }

        public async Task<Project> Add(Project project)
        {
            if (project is null)
                throw new ArgumentNullException(nameof(project));

            var existingProject = await _context.Projects.FirstOrDefaultAsync(u => u.Name == project.Name);

            if (existingProject != null)
                throw new InvalidOperationException("A project with the same name already exists.");

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> Delete(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project is null)
                return false;

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<Project> Update(Project project)
        {
            if (project is null)
                throw new ArgumentNullException(nameof(project));

            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(p => (p.Name == project.Name) && p.Id != project.Id);

            if (existingProject != null)
                throw new InvalidOperationException("A project with the same name already exists.");

            _context.Projects.Update(project);

            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<List<Project>> GetAllProjectsByOrganisationId(Guid id)
        {
            _logger.LogInformation("Fetching projects for organisation with ID: {OrganisationId}", id);
            return await _context.Projects
                .Where(p => p.OrganisationId == id)
                .Include(p => p.Owner)
                .ToListAsync();
        }

    }
}
