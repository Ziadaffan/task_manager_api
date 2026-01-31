using Application.Response.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Response.Project
{
    public class GetAllProjectResponse
    {
        public List<ProjectDto> Projects { get; set; }
    }
}
