using Application.Interfaces;
using Application.Requests.Organisation;
using Domaine.Classes;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationController : ControllerBase
    {
        private readonly IOrganisationService _organisationService;

        public OrganisationController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var organisations = await _organisationService.GetAll();
                return Ok(organisations);
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
                if (id == Guid.Empty)
                    return BadRequest("Id must be a non-empty GUID.");

                var organisation = await _organisationService.GetById(id);
                if (organisation == null)
                    return NotFound();

                return Ok(organisation);
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganisationRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request is null");

                var organisation = new Organisation(request.Name, request.Description);
                var created = await _organisationService.Add(organisation);

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
        public async Task<IActionResult> Update(Guid id, UpdateOrganisationRequest request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id must be a non-empty GUID.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingOrganisation = await _organisationService.GetById(id);
                if (existingOrganisation == null)
                    return NotFound("Organisation not found");

                existingOrganisation.Name = request.Name;
                existingOrganisation.Description = request.Description;

                var updated = await _organisationService.Update(existingOrganisation);
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

            var deleted = await _organisationService.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
