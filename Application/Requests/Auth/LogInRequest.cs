using System.ComponentModel.DataAnnotations;

namespace csharp_api.Requests.Auth
{
    public class LogInRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
