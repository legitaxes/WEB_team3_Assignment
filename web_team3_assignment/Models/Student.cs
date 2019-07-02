﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class Student
    {
        [Display(Name = "Student ID")]
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceed 50 Characters!")]
        public string Name { get; set; }

        [Display(Name="Course")]
        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceed 50 Characters!")]
        public string Course { get; set; }

        [Display(Name = "Photo")]
        [StringLength(255, ErrorMessage = "Name Cannot Exceed 255 Characters!")]
        public string Photo { get; set; }

        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "Name Cannot Exceed 3000 Characters!")]
        public string Description { get; set; }

        [Display(Name = "Achievement")]
        [StringLength(255, ErrorMessage = "Name Cannot Exceed 3000 Characters!")]
        public string Achievement { get; set; }

        [Display(Name = "External Link")]
        [StringLength(255, ErrorMessage = "Name Cannot Exceed 255 Characters!")]
        public string ExternalLink { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [ValidateEmailExists(ErrorMessage = "Email Address already exists!")]
        [StringLength(50, ErrorMessage = "Email Cannot Exceed 50 Characters!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password Length Cannot Exceed 255 Characters!")]
        public string Password { get; set; }

        [Display(Name = "Mentor ID")]
        public int MentorID { get; set; }
    }
}
