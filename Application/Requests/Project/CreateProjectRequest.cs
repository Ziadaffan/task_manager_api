using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Requests.Project
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public Guid OrganisationId { get; set; }
    }
}
