using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_team3_assignment.Models
{
    public class LecturerEdit
    {
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceed 50 Characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email Cannot Exceed 50 Characters!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Do Not Leave This Field Blank!")]
        [StringLength(255, ErrorMessage = "Password Length Cannot Exceed 255 Characters!")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(3000, ErrorMessage = "Description Cannot Exceed 3000 Characters!")]
        public string Description { get; set; }

    }
}
