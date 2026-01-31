using Domaine.Classes;
using Domaine.Enum;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Task = Domaine.Classes.Task;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Task> Add(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));


            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> Delete(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Task> GetById(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<Task> Update(Task task)
        {
            var existingTask = await _context.Tasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == task.Id);

            if (existingTask == null)
                throw new InvalidOperationException("Task not found");

            existingTask.AssignedUser = task.AssignedUser;

            await _context.SaveChangesAsync();
            return existingTask;
        }



        public async Task<Task> AssigneToUser(Task task, Domaine.Classes.User user)
        {
            task.AssignedUser = user;
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<Task> UpdateStatus(Task task, Status status)
        {
            task.TaskStatus = status;

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<IEnumerable<Task>> GetAll(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            var tasks = await _context.Tasks
                .Where(t => t.TaskProject.Id == project.Id)
                .Include(t => t.AssignedUser)
                .ToListAsync();

            return tasks;
        }

        public async Task<IEnumerable<Task>> GetAllForUser(Domaine.Classes.Project project, Domaine.Classes.User user)
        {
            var tasks = await _context.Tasks
                .Where(t => t.TaskProject.Id == project.Id && t.AssignedUser.Id == user.Id)
                .ToListAsync();

            return tasks;
        }
    }
}
