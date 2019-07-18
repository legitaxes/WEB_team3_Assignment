using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace web_team3_assignment.Models
{
    public class ProjectViewModel
    {
        //Still trying 

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

        [Required]
        [StringLength(255, ErrorMessage = "Project Title Cannot Exceed 255 Characters!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "Project Poster Name Cannot Exceed 255 Characters!")]
        [Display(Name = "Project Poster File Name")]
        public string ProjectPoster { get; set; }

        [StringLength(255, ErrorMessage = "URL Cannot Exceed 255 Characters!")]
        [Display(Name = "Project URL")]
        public string ProjectURL { get; set; }

        [StringLength(3000, ErrorMessage = "Project Description Cannot Exceed 3000 Characters!")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "ProjectMember")]
        public string ProjectMemberName { get; set; }

        public string projectphoto { get; set; }

        public IFormFile posterToUpload { get; set; }
    }
}
