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
    public class LecturerController : Controller
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SuggestionDAL suggestionContext = new SuggestionDAL();

        // GET: Lecturer
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) || 
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Lecturer> lecturerList = lecturerContext.GetAllLecturer();
            return View(lecturerList);
        }

        //Done
        // GET: Lecturer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //Done
        // GET: Lecturer/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) || 
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //Done

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lecturer lecturer)
        {
            lecturer.Password = "p@55Lecturer";
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Employee Created Successfully!";
                lecturer.LecturerId = lecturerContext.Add(lecturer);
                return View();
            }
            else
            {
                return View(lecturer);
            }
            //try
            //{
            //    // TODO: Add insert logic here

            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //GET: Lecturer/Change Function
        public ActionResult Change()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
               (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            //get current lecturer ID through session get string
            int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            //get all the lecturer details based on the ID
            //LecturerPassword lecturer = lecturerContext.getPasswordDetails(id);
            LecturerPassword lecturer = new LecturerPassword();
            lecturer.LecturerId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            //if (lecturer == null)
            //{
            //    return RedirectToAction("Index");
            //}
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(LecturerPassword lecturer)
        {
            //get password details for currently logged in lecturer
            LecturerPassword currentLecturer = lecturerContext.getPasswordDetails(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            //if the password the user key in DOES NOT match the database password...
            if (lecturer.Password != currentLecturer.Password)
            {
                ViewData["Message"] = "Current Password Is Incorrect!";
                return View(lecturer);
            }
            //check whether the password field is empty
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            System.Diagnostics.Debug.WriteLine("password: " + lecturer.Password);
            System.Diagnostics.Debug.WriteLine("new pw: " + lecturer.NewPassword);
            System.Diagnostics.Debug.WriteLine("confirm pw: " + lecturer.ConfirmPassword);

            if (ModelState.IsValid)
            {
                //checks whether the password is the same
                if (lecturer.NewPassword == lecturer.ConfirmPassword)
                {
                    if (lecturerContext.ChangePassword(lecturer))
                    {
                        ViewData["Message"] = "Password Changed Successfully!";
                        return View(lecturer);
                    }
                }
                //if password does not match
                else
                {
                    ViewData["Message"] = "Password Does Not Match!";
                    return View(lecturer);
                }
            }
            //if password field is empty OR does not match the required model from Lecturer.cs, return to view with error message
            ViewData["Message"] = "Password Field Did Not Meet Requirements!";
            return View(lecturer);
        }

        // GET: Lecturer/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //        (HttpContext.Session.GetString("Role") != "Lecturer"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    if (id == null)
        //    {
        //        return RedirectToAction("Index");   
        //    }
        //    Lecturer lecturer = lecturerContext.getLecturerDetails(id.Value);
        //    if (lecturer == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(lecturer);
        //}

        //// POST: Lecturer/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Lecturer/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            //get all the lecturer details based on the ID
            Lecturer lecturer = lecturerContext.getLecturerDetails(id.Value);
            //check if lecturer object is empty or the lecturerID matches the currently logged in user
            if (lecturer == null || Convert.ToInt32(HttpContext.Session.GetString("ID")) != id.Value)
            {
                TempData["ErrorMessage"] = "You are not allowed to delete other lecturer's profile!";
                return RedirectToAction("Index");
            }

            //checks whether the mentorID is under another student
            //if its true
            if (lecturerContext.CheckIsUsed(Convert.ToInt32(HttpContext.Session.GetString("ID"))))
            {
                TempData["ErrorMessage"] = "You are not allowed to delete your profile! There are still students are under your profile!";
                return RedirectToAction("Index");
            }
            return View(lecturer);
        }

        // POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Lecturer lecturer)
        {
            // Delete the staff record from database
            lecturerContext.Delete(lecturer.LecturerId);
            //Clear session state and log user out
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index", "Home");
        }

        //GET: Lecturer/Mentees
        public ActionResult Mentee()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Student> studentList = lecturerContext.GetMenteeDetails(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            return View(studentList);
        }
    }
}