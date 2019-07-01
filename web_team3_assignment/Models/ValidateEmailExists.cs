using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using web_team3_assignment.DAL;

namespace web_team3_assignment.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private LecturerDAL lecturerContext = new LecturerDAL();

        public override bool IsValid(object value)
        {
            string email = Convert.ToString(value);
            if (lecturerContext.IsEmailExist(email))
                return false; // validation failed
            else
                return true; // validation passed }
        }
    }
}
