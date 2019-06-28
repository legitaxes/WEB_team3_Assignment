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

            if (homeContext.lecturerLogin(loginID, password))
            {
                HttpContext.Session.SetString("LoginID", loginID);
                HttpContext.Session.SetString("Role", "Lecturer");
                HttpContext.Session.SetString("currentTime", DateTime.Now.ToString());
                // Redirect user to the "StaffMain" view through an action
                return RedirectToAction("LecturerMain");
            }
            else
            {
                TempData["Message"] = "Invalid Login Credentials!";
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
                HttpContext.Session.SetString("LoginID", studentLoginID);
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

            //if (loginID == "abc2@npbook.com" && password == "pass1234")
            //{
            //    HttpContext.Session.SetString("LoginID", loginID);
            //    HttpContext.Session.SetString("Role", "Student");
            //    HttpContext.Session.SetString("currentTime", DateTime.Now.ToString());
            //    // Redirect user to the "StudentMain" view through an action
            //    return RedirectToAction("StudentMain");
            //}

            //else
            //{
            //    // Store an error message in TempData for display at the index view
            //    TempData["Message"] = "Invalid Login Credentials!";

            //    // Redirect user back to the index view through an action
            //    return RedirectToAction("Index");
            //}
        }

        public ActionResult LecturerMain()
        {
            return View();
        }

        public ActionResult StudentMain()
        {
            return View();
        }


        public ActionResult ProjectMain()
        {
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