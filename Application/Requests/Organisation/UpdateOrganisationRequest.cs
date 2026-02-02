using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Organisation
{
    public class UpdateOrganisationRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
