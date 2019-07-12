using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class UpdateTitle
    {
        [Required(ErrorMessage = "Please Do not Leave Title Field Blank!")]
        [StringLength(255, ErrorMessage = "Project Title Cannot Exceed 255 Characters!")]
        // Custom Validation Attribute for checking Title exists
        [ValidateTitleExists(ErrorMessage = "Title already exists!")]
        [Display(Name = "Title")]
        public string Title { get; set; }
    }
}
