﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace web_team3_assignment.Controllers
{
    public class HomeController : Controller
    {
        private HomeDAL homeContext = new HomeDAL();
        private StudentDAL studentContext = new StudentDAL();
        private LecturerDAL lecturerContext = new LecturerDAL();

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
            //hashing the keyed in password
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            //convert to a string
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            Lecturer lecturer = homeContext.lecturerLogin(loginID, hashedPassword);
            //var sha1Password = sha1.ComputeHash(Encoding.UTF8.GetBytes(lecturer.Password));
            //System.Diagnostics.Debug.WriteLine(sha1Password);
            if (lecturer.Email == loginID && lecturer.Password == hashedPassword)
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
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(studentPassword));
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            Student student = homeContext.studentLogin(studentLoginID, hashedPassword);

            if (student.EmailAddr == studentLoginID && student.Password == hashedPassword)
            {
                HttpContext.Session.SetString("LoginName", student.Name);
                HttpContext.Session.SetInt32("StudentID", student.StudentID);
                HttpContext.Session.SetInt32("StudentsMentorID", student.MentorID);
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
            // HomeController's homeContext will getProjectID through HomeDAL by student's studentID
            ProjectMember projectmember = homeContext.getProjectID(HttpContext.Session.GetInt32("StudentID").Value);
            if (projectmember.Role != null || projectmember.ProjectId != 0)
            {
                HttpContext.Session.SetInt32("ProjectID", projectmember.ProjectId);
                HttpContext.Session.SetString("ProjectRole", projectmember.Role);
                return View(projectmember);
            }
            return View(projectmember);
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

        //UPDATE COURSE
        private List<SelectListItem> GetCourse()
        {
            List<SelectListItem> Course = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "Select Course"
                },
                new SelectListItem
                {
                    Value = "IT",
                    Text = "Information Technology"
                },
                new SelectListItem
                {
                    Value = "FI",
                    Text = "Financial Infomatics"
                }
            };

            return Course;
        }

        //UPDATE COURSE
        private List<SelectListItem> GetLecturer()
        {
            List<SelectListItem> Lecturer = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "Select Mentor"
                }
            };
            List<Lecturer> lecturerList = lecturerContext.GetAllLecturer();
            foreach (Lecturer lecturer in lecturerList)
            {
                Lecturer.Add(
                new SelectListItem
                {
                    Value = lecturer.LecturerId.ToString(),
                    Text = lecturer.Name
                });
            }
            return Lecturer;
        }

        public ActionResult CreateStudent()
        {
            ViewData["CourseSelect"] = GetCourse();
            ViewData["LecturerSelect"] = GetLecturer();
            return View();
        }

        // POST: Create student profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(StudentCreate student)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes("p@55Student"));
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            student.Password = hashedPassword;
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            ViewData["CourseSelect"] = GetCourse();
            ViewData["LecturerSelect"] = GetLecturer();
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Student profile Created Successfully!";
                student.StudentID = studentContext.Add(student);
                return View();
            }
            else
            {
                return View(student);
            }
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