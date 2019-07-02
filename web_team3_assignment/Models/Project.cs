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
        //[ValidateProjectExists(ErrorMessage = "ProjectId already exists!")]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Project Title Cannot Exceed 255 Characters!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(3000, ErrorMessage = "Project Description Cannot Exceed 3000 Characters!")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Project Poster Name Cannot Exceed 255 Characters!")]
        [Display(Name = "Project Poster File Name")]
        public string ProjectPoster { get; set; }

        [StringLength(255, ErrorMessage = "URL Cannot Exceed 255 Characters!")]
        [Display(Name = "Project URL")]
        public string ProjectURL { get; set; }
    }
}
