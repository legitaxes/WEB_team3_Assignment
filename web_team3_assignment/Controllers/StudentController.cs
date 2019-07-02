using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;

namespace web_team3_assignment.Controllers
{
    public class StudentController : Controller
    {
        private StudentDAL studentContext = new StudentDAL();

        public IActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Student> studentList = studentContext.GetAllStudent();
            return View(studentList);
        }
    }
}