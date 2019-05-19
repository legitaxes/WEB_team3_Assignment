using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web_team3_assignment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StaffLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            if (loginID == "abc@npbook.com" && password == "pass1234")
            {
                HttpContext.Session.SetString("LoginID", loginID);
                HttpContext.Session.SetString("Role", "Lecturer");
                // Redirect user to the "StaffMain" view through an action
                return RedirectToAction("LecturerMain");
            }

            if (loginID == "abc2@npbook.com" && password == "pass1234")
            {
                HttpContext.Session.SetString("LoginID", loginID);
                HttpContext.Session.SetString("Role", "Student");
                // Redirect user to the "StudentMain" view through an action
                return RedirectToAction("StudentMain");
            }

            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                // Redirect user back to the index view through an action
                return RedirectToAction("Index");
            }
        }

        public ActionResult LecturerMain()
        {
            return View();
        }

        public ActionResult StudentMain()
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