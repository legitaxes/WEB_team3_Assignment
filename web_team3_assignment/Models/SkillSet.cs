using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web_team3_assignment.Models
{
    public class SkillSet
    {
        //[Key]
        //[Required(ErrorMessage = "Please Do Not Leave This Field Blank!")]
        [Display(Name = "SkillSet ID")]
        public int SkillSetId { get; set; }

        [Required(ErrorMessage = "Please do not leave this field Blank!")]
        [StringLength(255, ErrorMessage = "Skill Set Name Cannot Exceed 255 Characters!")]
        // Custom Validation Attribute for checking SkillSet exists
        [ValidateSkillSetExists(ErrorMessage = "SkillSet already exists!")]
        [Display(Name = "SkillSet Name")]
        public string SkillSetName { get; set; }
    }
}
