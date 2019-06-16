using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class Lecturer
    {
        [Key]
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(50, ErrorMessage = "Cannot Exceed 50 Characters!")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        // Custom Validation Attribute for checking email address exists
        //[ValidateEmailExists(ErrorMessage = "Email address already exists!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
