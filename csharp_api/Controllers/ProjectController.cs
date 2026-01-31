using Application.Interfaces;
using Application.Requests.Project;
using Application.Response.DTOs;
using Application.Response.Project;
using Domaine.Classes;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var projects = await _projectService.GetAll();

                var response = new GetAllProjectResponse
                {
                    Projects = projects.Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        CreatedAt = p.CreatedAt,
                        OwnerId = p.Owner.Id
                    }).ToList()
                };

                return Ok(response.Projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Organisation/{id:guid}")]
        public async Task<IActionResult> GetAllProjectsByOrganisationId(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id must be a non-empty GUID.");

                var project = await _projectService.GetAllProjectsByOrganisationId(id);
                if (project == null)
                    return NotFound();

                return Ok(project);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var project = await _projectService.GetById(id);
                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectRequest request)
        {
            try
            {
                if (request == null)
                    return Unauthorized("User ID not found in token.");


                var user = await _userService.GetById(request.UserId);

                

                var project = new Project(request.Name, request.Description, user, request.OrganisationId);
                var created = await _projectService.Add(project);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectRequest request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id must be a non-empty GUID.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProject = await _projectService.GetById(id);
                if (existingProject == null)
                    return NotFound("project not found");

                existingProject.Name = request.Name;
                existingProject.Description = request.Description;

                var updated = await _projectService.Update(existingProject);
                return Ok(updated);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id must be a non-empty GUID.");

            var deleted = await _projectService.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
