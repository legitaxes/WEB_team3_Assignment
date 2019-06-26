﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class SkillSet
    {
        [Key]
        [Required(ErrorMessage = "Please Do Not Leave This Field Blank!")]
        [Display(Name = "SkillSet ID")]
        public int SkillSetId { get; set; }

        [Required(ErrorMessage = "Please Do not Leave This Field Blank!")]
        [StringLength(255, ErrorMessage = "Skill Set Name Cannot Exceed 255 Characters!")]
        [Display(Name = "SkillSet Name")]
        public string SkillSetName { get; set; }
    }
}