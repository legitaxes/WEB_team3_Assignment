using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class Project
    {
        [Key]
        [Required]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Please Do not Leave Title Field Blank!")]
        [StringLength(255, ErrorMessage = "Project Title Cannot Exceed 255 Characters!")]
        //// Custom Validation Attribute for checking Title exists
        //[ValidateTitleExists(ErrorMessage = "Title already exists!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Do not Leave Description Field Blank!")]
        [StringLength(3000, ErrorMessage = "Project Description Cannot Exceed 3000 Characters!")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Do not Leave ProjectPoster Field Blank!")]
        [StringLength(255, ErrorMessage = "Project Poster Name Cannot Exceed 255 Characters!")]
        [Display(Name = "Project Poster File Name")]
        public string ProjectPoster { get; set; }

        [Required(ErrorMessage = "Please Do not Leave ProjectURL Field Blank!")]
        [StringLength(255, ErrorMessage = "URL Cannot Exceed 255 Characters!")]
        [Display(Name = "Project URL")]
        public string ProjectURL { get; set; }
    }
}
