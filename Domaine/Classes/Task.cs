using Domaine.Enum;

namespace Domaine.Classes
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = null;
        public Status TaskStatus { get; set; }
        public int Priority { get; set; }
        public DateOnly DueDate { get; set; }
        public Project TaskProject { get; set; }
        public User? AssignedUser { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Task(string title, string? description, Project taskProject, DateOnly dueDate, int priority, User? user = null, Status status = Status.Backlog)
        {
            Title = title;
            Description = description;
            TaskProject = taskProject;
            TaskStatus = status;
            DueDate = dueDate;
            Priority = priority;
            AssignedUser = user;
            CreatedAt = DateTime.UtcNow;
        }

        public Task()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
