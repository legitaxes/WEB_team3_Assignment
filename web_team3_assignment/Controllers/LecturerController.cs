using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;
using System.Diagnostics;

namespace web_team3_assignment.Controllers
{
    public class LecturerController : Controller
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SuggestionDAL suggestionContext = new SuggestionDAL();

        //Gets all Lecturer details and display it in a table
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

        //Create Lecturer Page
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
        //Create Lecturer Page
        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lecturer lecturer)
        {
            //set the default password for the lecturer account to 'p@55Mentor' but hashed
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(lecturer.Password));
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            lecturer.Password = hashedPassword;
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Lecturer Created Successfully!";
                lecturer.LecturerId = lecturerContext.Add(lecturer);
                return View();
            }
            else
            {
                return View(lecturer);
            }
        }

        //Change Password Page
        //GET: Lecturer/Change Function
        public ActionResult Change()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
               (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            //set a variable from the session string logged in Lecturer's ID
            int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            //get all the lecturer details based on the ID
            LecturerPassword lecturer = new LecturerPassword();
            lecturer.LecturerId = id;
            return View(lecturer);
        }
        //Change Password Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(LecturerPassword lecturer)
        {
            if (lecturer.Password == null)
            {
                return View(lecturer);
            }
            //get password details for currently logged in lecturer
            LecturerPassword currentLecturer = lecturerContext.getPasswordDetails(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(lecturer.Password));
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            //if password DOES NOT match the database password...
            if (hashedPassword != currentLecturer.Password)
            {
                ViewData["Message"] = "Current Password Is Incorrect!";
                return View(lecturer);
            }
            //else continue what is needed to be done
            if (ModelState.IsValid)
            {
                //checks whether the password is the same
                if (lecturer.NewPassword == lecturer.ConfirmPassword)
                {
                    //Checks the password whether it contains a digit, hashes the password using SHA-1 and updates the password into the database
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

        //Edit Lecturer Profile Page
        //GET: Lecturer/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            //set a variable from the session string logged in Lecturer's ID
            int lecturerid = Convert.ToInt32(HttpContext.Session.GetString("ID"));

            //check if id is null or if the id matches the currently logged in user
            if (id == null || lecturerid != id.Value)
            {
                return RedirectToAction("Index");
            }
            //get the lecturer details and prepare to return it to the edit view
            LecturerEdit lecturer = lecturerContext.EditLecturerDetails(id.Value);
            //check whether lecturer actually exists and whether the ID matches the logged in lecturer ID
            if (lecturer == null || lecturerid != lecturer.LecturerId)
            {
                TempData["ErrorMessage"] = "You are not allowed to edit other lecturer's profile!";
                return RedirectToAction("Index");
            }
            //if everything is ok, return the lecturer object to the view
            return View(lecturer);
        }

        // Edit Lecturer's Profile Page
        // POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LecturerEdit lecturer)
        {
            if (ModelState.IsValid)
            { //Update staff record to database 
                lecturerContext.Update(lecturer);
                HttpContext.Session.SetString("LoginName", lecturer.Name.ToString());
                return RedirectToAction("Index");
            }
            ViewData["Message"] = "Check your fields again!";
            return View(lecturer);
        }

        // GET: Lecturer/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }

            //set a variable from the session string logged in Lecturer's ID
            int lecturerid = Convert.ToInt32(HttpContext.Session.GetString("ID"));

            //check whether the id is null or not
            if (id == null)
            {
                TempData["ErrorMessage"] = "You can only delete your own profile!";
                return RedirectToAction("Index");
            }

            //get all the lecturer details based on the ID
            Lecturer lecturer = lecturerContext.getLecturerDetails(id.Value);
            //check if lecturer object is empty or the lecturerID matches the currently logged in user
            if (lecturer == null || lecturerid != id.Value)
            {
                TempData["ErrorMessage"] = "You are not allowed to delete other lecturer's profile!";
                return RedirectToAction("Index");
            }

            //checks whether the mentorID is under another student
            //if its true
            if (lecturerContext.CheckIsUsed(lecturerid))
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
            //gets a list of student using GetMenteeDetails method
            List<Student> studentList = lecturerContext.GetMenteeDetails(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            return View(studentList);
        }
    }
}