using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class ProjectMember
    {
        [Required]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Role")]
        [StringLength(50, ErrorMessage = "Cannot Exceed 50 Characters")]
        public string Role { get; set; }
    }
}
