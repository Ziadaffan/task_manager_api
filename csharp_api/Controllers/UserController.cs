using Application.Interfaces;
using csharp_api.Requests.User;
using Domaine.Classes;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrganisationService _organisationService;

        public UserController(IUserService userService, IOrganisationService organisationService)
        {
            _userService = userService;
            _organisationService = organisationService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetById(id);

                if (user is null) return NotFound();

                return user;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the user.");

            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest req)
        {
            try
            {
                if (req is null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password)) return BadRequest("invalid parametre");

                var organisation = (Organisation?)null;

                if (req.OrganisationId != Guid.Empty)
                {
                    organisation = await _organisationService.GetById(req.OrganisationId);

                }

                var user = new User(req.Username.ToLower(), req.Email.ToLower(), req.Password.ToLower(), organisation);

                var createdUser = await _userService.Add(user);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpGet("Organisation/{organisationId:guid}")]
        public async Task<IActionResult> GetUsersByOrganisationId(Guid organisationId)
        {
            try
            {
                var users = await _userService.GetByOrganisationId(organisationId);
                return Ok(users);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest req)
        {
            try
            {
                if (req is null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Email)) return BadRequest();

                var user = await _userService.GetById(id);

                if (user is null) return NotFound("user not found");

                user.Username = req.Username.ToLower();
                user.Email = req.Email.ToLower();
                user.Organisation = await _organisationService.GetById(req.OrganisationId);

                var updatedUser = await _userService.Update(user);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
