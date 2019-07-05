using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using web_team3_assignment.DAL;

namespace web_team3_assignment.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        private StudentDAL studentContext = new StudentDAL();
        public override bool IsValid(object value)
        {
            string email = Convert.ToString(value);
            if (lecturerContext.IsEmailExist(email))
            {
                return false; // validation failed
            }
            if (studentContext.IsEmailExist(email, 1))
            {
                return false;
            }
            else
                return true; // validation passed }
        }
    }
}
