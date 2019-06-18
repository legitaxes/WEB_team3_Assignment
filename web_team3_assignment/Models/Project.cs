using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class Project
    {
        [Required]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Project Poster")]
        public string ProjectPoster { get; set; }

        [Required]
        [Display(Name = "Project URL")]
        public string ProjectURL { get; set; }
    }
}
