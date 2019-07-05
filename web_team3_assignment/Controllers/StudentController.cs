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

        public ActionResult Update(int id)
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

        public ActionResult UpdatePhoto(int id)
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role in
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            Student student = studentContext.GetStudentDetails(studentid);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhoto(Student student)
        {
            if (student.FileToUpload != null && student.FileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(student.FileToUpload.FileName);
                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = student.Name + fileExt; 
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot\\images", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(savePath, FileMode.Create))
                    {
                        await student.FileToUpload.CopyToAsync(fileSteam);
                    }
                    student.Photo = uploadedFile;
                    ViewData["Message"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied 
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex)
                //Other type of error 
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(student);
        }
    }
}