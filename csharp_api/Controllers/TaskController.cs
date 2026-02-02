using Application.Interfaces;
using Application.Requests.Task;
using Domaine.Classes;
using Domaine.Enum;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Task = Domaine.Classes.Task;

namespace csharp_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, IProjectRepository projectRepository, IUserRepository userRepository, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Title) || req.ProjectId == Guid.Empty || !Enum.IsDefined(typeof(Status), (Status)req.TaskStatus)) return BadRequest();

            var existingProject = await _projectRepository.GetById(req.ProjectId);
            if (existingProject == null) return NotFound($"Project with ID {req.ProjectId} not found.");

            var existingUser = (User?)null;

            _logger.LogInformation("UserId in request: {UserId}", req.UserId);
            if (req.UserId != Guid.Empty)
            {
                existingUser = await _userRepository.GetById(req.UserId);
                if (existingUser == null) return NotFound($"User with ID {req.UserId} not found.");
            }



            var created = await _taskService.Add(new Task(req.Title, req.Description, existingProject, DateOnly.FromDateTime(req.DueDate), req.Priority, existingUser, req.TaskStatus));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetById(id);
            return task != null ? Ok(task) : NotFound();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Title)) return BadRequest();
            var existingTask = await _taskService.GetById(id);
            if (existingTask == null) return NotFound();

            var existingUser = (User?)null;

            if (existingUser != null)
            {
                existingUser = await _userRepository.GetById(req.UserId);
                if (existingUser == null) return NotFound($"User with ID {req.UserId} not found.");
            }

            existingTask.Title = req.Title;
            existingTask.Description = req.Description;
            existingTask.TaskStatus = req.TaskStatus;
            existingTask.Priority = req.Priority;
            existingTask.DueDate = DateOnly.FromDateTime(req.DueDate);
            existingTask.AssignedUser = existingUser;


            var updated = await _taskService.Update(existingTask);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _taskService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the task.");
            }
        }

        [HttpPut("{id:guid}/assign/{userId:guid}")]
        public async Task<IActionResult> AssignToUser(Guid id, Guid userId)
        {
            try
            {
                if (id == Guid.Empty || userId == Guid.Empty) return BadRequest("Invalid ID.");
                var task = await _taskService.GetById(id);
                if (task == null) return NotFound();

                var user = await _userRepository.GetById(userId);
                var updated = await _taskService.AssigneToUser(task, user);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning task to user");
                return StatusCode(500, "An error occurred while assigning the task to the user.");
            }

        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] Status status)
        {
            try
            {
                var task = await _taskService.GetById(id);
                if (task == null) return NotFound();

                var updated = await _taskService.UpdateStatus(task, status);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the task status.");

            }
        }

        [HttpGet("project/{projectId:guid}")]
        public async Task<IActionResult> GetAllForProject(Guid projectId)
        {
            try
            {
                var project = new Project { Id = projectId };
                var tasks = await _taskService.GetAll(project);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving tasks for the project.");
            }

        }

        [HttpGet("project/{projectId:guid}/user/{userId:guid}")]
        public async Task<IActionResult> GetAllForProjectAndUser(Guid projectId, Guid userId)
        {
            try
            {
                var project = new Project { Id = projectId };
                var user = new User { Id = userId };
                var tasks = await _taskService.GetAllForUser(project, user);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving tasks for the project and user.");
            }

        }
    }
}
