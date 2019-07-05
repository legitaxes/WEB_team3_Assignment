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

        public IActionResult Index(int? id)
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Lecturer" role
            System.Diagnostics.Debug.WriteLine((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student") || (HttpContext.Session.GetString("Role") != "Lecturer"));

            if ((HttpContext.Session.GetString("Role") == null))
            {
                return RedirectToAction("Index", "Home");
            }
            if ((HttpContext.Session.GetString("Role") == "Student") || (HttpContext.Session.GetString("Role") == "Lecturer"))
            {
                if (id == null)
                {
                    int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
                    Student student = studentContext.GetStudentDetails(studentid);
                    return View(student);
                }
                else
                {
                    Student Lecturerstudent = studentContext.GetStudentDetails(id.Value);
                    return View(Lecturerstudent);
                }
                //Student student = studentContext.GetStudentDetails(studentid);
                //return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Update()
        {
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role

            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            Student student = studentContext.GetStudentDetails(studentid);

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Student student)
        {
            if (ModelState.IsValid)
            {
                studentContext.UpdateProfile(student);
                return RedirectToAction("Update");
            }
            return View("Update");
        }
    }
}