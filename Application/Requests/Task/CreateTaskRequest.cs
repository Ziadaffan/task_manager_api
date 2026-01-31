using Domaine.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Requests.Task
{
    public class CreateTaskRequest
    {
        public  string Title { get; set; }
        public  string Description { get; set; }
        public Guid ProjectId { get; set; }
        public Status TaskStatus { get; set; }
        public int Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
    }
}
