using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class Suggestion
    {
        [Display(Name = "Suggestion ID")]
        public int SuggestionId { get; set; }

        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [StringLength(3000, ErrorMessage = "Description Cannot Exceed 3000 Characters!")]
        public string Description { get; set; }

        [Required]
        [StringLength(1, ErrorMessage ="Status Cannot Exceed More Than 1 Character!")]
        public char Status { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DateCreated { get; set; }
    }
}
