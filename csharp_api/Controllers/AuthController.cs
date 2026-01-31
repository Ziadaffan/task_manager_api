using Application.Interfaces;
using csharp_api.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInRequest req)
        {
            try
            {
                var response = await _userService.LogIn(req.Username.ToLower(), req.Password.ToLower());
                if (response == null)
                {
                    return Unauthorized();
                }
                
                return Ok(new { token = response.Token, role = response.Role });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
