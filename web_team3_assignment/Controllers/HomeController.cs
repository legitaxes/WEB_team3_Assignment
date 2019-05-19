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
                // Redirect user to the "StaffMain" view through an action
                return RedirectToAction("LecturerMain");
            }

            if (loginID == "abc2@npbook.com" && password == "pass1234")
            {
                // Redirect user to the "StudentMain" view through an action
                return RedirectToAction("StudentMain");
            }

            else
            {
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
    }
}