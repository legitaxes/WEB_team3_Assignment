using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_team3_assignment.Models
{
    public class StudentUpdatePassword
    {
        [Display(Name = "Lecturer ID")]
        public int StudentID { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(255, ErrorMessage = "Password Length Cannot Exceed 255 Characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [Display(Name = "New Password:")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password Length Cannot Exceed 255 Characters!")]
        [MinLength(8, ErrorMessage = "Password Should Be At least 8 Characters Long!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [Display(Name = "Confirm Password:")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password Length Cannot Exceed 255 Characters!")]
        public string ConfirmPassword { get; set; }
    }
}
