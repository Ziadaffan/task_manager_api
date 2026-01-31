using Application.Interfaces;
using Domaine.Classes;
using Domaine.Enum;
using Infrastructure.Interface;

namespace Application.Services
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Domaine.Classes.Task> Add(Domaine.Classes.Task task)
        {
            return await _taskRepository.Add(task);
        }

        public async Task<Domaine.Classes.Task> AssigneToUser(Domaine.Classes.Task task, User user)
        {
            var existingTask = await _taskRepository.GetById(task.Id);
            if (existingTask == null) throw new KeyNotFoundException("Task not found");
            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("User not found");

            existingTask.AssignedUser = existingUser;

            return await _taskRepository.Update(existingTask);
        }

        public async Task<bool> Delete(Guid id)
        {
            var existingTask = await _taskRepository.GetById(id);

            if (existingTask == null) throw new KeyNotFoundException("Task not found");

            return await _taskRepository.Delete(id);
        }

        public async Task<IEnumerable<Domaine.Classes.Task>> GetAll(Domaine.Classes.Project project)
        {
            var existingProject = await _projectRepository.GetById(project.Id);

            if (existingProject == null) throw new KeyNotFoundException("Project not found");

            return await _taskRepository.GetAll(existingProject);
        }

        public async Task<IEnumerable<Domaine.Classes.Task>> GetAllForUser(Domaine.Classes.Project project, User user)
        {
            var existingProjectTask = await _projectRepository.GetById(project.Id);
            if (existingProjectTask == null) throw new KeyNotFoundException("Project not found");
            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("User not found");

            return await _taskRepository.GetAllForUser(existingProjectTask, existingUser);
        }

        public async Task<Domaine.Classes.Task> GetById(Guid id)
        {
            return await _taskRepository.GetById(id);
        }

        public async Task<Domaine.Classes.Task> Update(Domaine.Classes.Task entity)
        {
            return await _taskRepository.Update(entity);
        }

        public async Task<Domaine.Classes.Task> UpdateStatus(Domaine.Classes.Task task, Status status)
        {
            var existingTask = await _taskRepository.GetById(task.Id);

            if (existingTask == null) throw new KeyNotFoundException("Task not found");

            existingTask.TaskStatus = status;

            return await _taskRepository.Update(existingTask);
        }
    }
}
