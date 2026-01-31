using Application.Interfaces;
using Domaine.Classes;
using Infrastructure.Interface;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<List<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public async Task<List<Project>> GetAllProjectsByOrganisationId(Guid id)
        {
            return await _projectRepository.GetAllProjectsByOrganisationId(id);
        }
        public async Task<Project> GetById(Guid id)
        {
            return await _projectRepository.GetById(id);
        }
        public async Task<Project> Add(Project project)
        {
            return await _projectRepository.Add(project);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _projectRepository.Delete(id);
        }

        public async Task<Project> Update(Project entity)
        {
            return await _projectRepository.Update(entity);
        }
    }
}
