using Domaine.Classes;
using Domaine.Enum;
using Task = Domaine.Classes.Task;

namespace Infrastructure.Interface
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        Task<Task> UpdateStatus(Task task, Status status);
        Task<Task> AssigneToUser(Task task, User user);
        Task<IEnumerable<Task>> GetAll(Project project);
        Task<IEnumerable<Task>> GetAllForUser(Project project, User user);

    }
}
