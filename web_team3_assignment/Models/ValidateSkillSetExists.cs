using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using web_team3_assignment.DAL;

namespace web_team3_assignment.Models
{
    public class ValidateSkillSetExists : ValidationAttribute
    {
        private SkillSetDAL SkillSetContext = new SkillSetDAL();
        public override bool IsValid(object value)
        {
            string skillset = Convert.ToString(value);
            if (SkillSetContext.IsSkillSetExist(skillset))
                return false; // validation failed
            else
                return true; // validation passed 
        }
    }
}
