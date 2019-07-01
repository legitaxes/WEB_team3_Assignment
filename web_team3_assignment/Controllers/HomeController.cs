using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;
using System.IO;


namespace web_team3_assignment.Controllers
{
    public class HomeController : Controller
    {
        private HomeDAL homeContext = new HomeDAL();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StaffLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLecturerID"].ToString().ToLower();
            string password = formData["txtLecturerPassword"].ToString();
            Lecturer lecturer = homeContext.lecturerLogin(loginID, password);
            if (lecturer.Email == loginID && lecturer.Password == password)
            {
                HttpContext.Session.SetString("LoginName", lecturer.Name.ToString());
                HttpContext.Session.SetString("ID", lecturer.LecturerId.ToString());
                HttpContext.Session.SetString("Role", "Lecturer");
                HttpContext.Session.SetString("currentTime", DateTime.Now.ToString());
                // Redirect user to the "StaffMain" view through an action
                return RedirectToAction("LecturerMain");
            }
            else
            {
                TempData["LecturerMessage"] = "Invalid Login Credentials!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult StudentLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string studentLoginID = formData["txtLoginID"].ToString().ToLower();
            string studentPassword = formData["txtPassword"].ToString();

            if (homeContext.studentLogin(studentLoginID, studentPassword))
            {
                HttpContext.Session.SetString("LoginName", studentLoginID);
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetString("currentTime", DateTime.Now.ToString());
                // Redirect user to the "StudentMain" view through an action
                return RedirectToAction("StudentMain");
            }
            else
            {
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("Index");
            }
        }

        public ActionResult LecturerMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult StudentMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult ProjectMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult SkillSetMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }
    }
}