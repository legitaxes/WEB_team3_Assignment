using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class ProjectMemberViewModel
    {
        public List<ProjectMember> projectMemberList { get; set; }
        public List<Project> projectList { get; set; }
    }
}
