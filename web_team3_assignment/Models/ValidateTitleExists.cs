using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using web_team3_assignment.DAL;

namespace web_team3_assignment.Models
{
    public class ValidateTitleExists : ValidationAttribute
    {
        private ProjectDAL projectContext = new ProjectDAL();
        public override bool IsValid(object value)
        {
            string Title = Convert.ToString(value);
            if (projectContext.IsProjectTitleExists(Title))
                return false; // validation failed
            else
                return true; // validation passed 
        }
    }
}
