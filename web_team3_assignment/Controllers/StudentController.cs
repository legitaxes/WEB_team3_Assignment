﻿using System;
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
using System.IO;

namespace web_team3_assignment.Controllers
{
    public class StudentController : Controller
    {
        private StudentDAL studentContext = new StudentDAL();
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SkillSetDAL skillsetContext = new SkillSetDAL();
        private SuggestionDAL suggestionContext = new SuggestionDAL();
        private StudentSkillSetDAL studentskillsetContext = new StudentSkillSetDAL();
        //Retrieves list of courses
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

        //Retrieves list of lecturers
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

        //Gets all Student details and display it in a table
        // GET: Student
        public async Task<IActionResult> Browse(string searchString)
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Student" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["CurrentFilter"] = searchString;
            List<Student> studentList = studentContext.GetAllStudent();
            var students = from s in studentList
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
                return View(students.ToList());
            }
            else
            {
                return View(studentList);
            }
        }

        public ActionResult ViewPeer(int id)
        {
            //View peer Profile Page
            //GET: Student/ViewPeer/id
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            StudentViewModel studentList = new StudentViewModel();
            List<Student> allstudentList = studentContext.GetAllStudent();
            foreach (Student student in allstudentList)
            {
                if (student.StudentID == id)
                {
                    StudentViewModel StudentVM = MapToLecturer(student);
                    studentList = StudentVM;
                }
            }
            List<StudentSkillSetViewModel> allskillsetList = studentskillsetContext.GetAllSkillSets();
            allskillsetList = studentskillsetContext.GetStudentsSkillSet(id);
            ViewBag.List = allskillsetList;
            //if everything is ok, return the lecturer object to the view
            return View(studentList);
        }
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
                //check if the query string is empty, if it is empty, it displays under the student's context
                if (id == null)
                {
                    int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
                    //Student student = studentContext.GetStudentDetails(studentid);
                    StudentViewModel studentList = new StudentViewModel();
                    List<Student> allstudentList = studentContext.GetAllStudent();
                    foreach (Student student in allstudentList)
                    {
                        if (student.StudentID == studentid)
                        {   
                            StudentViewModel StudentVM = MapToLecturer(student);
                            studentList = StudentVM;
                        }
                    }
                    List<StudentSkillSetViewModel> allskillsetList = studentskillsetContext.GetAllSkillSets();
                    allskillsetList = studentskillsetContext.GetStudentsSkillSet(studentid);
                    ViewBag.list = allskillsetList;
                    System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("ID"));
                    if (studentList == null)
                    {
                        TempData["ErrorMessage"] = "Mentee does not exist!";
                        return RedirectToAction("Mentee", "Lecturer");
                    }
                    return View(studentList);
                }
                //display under lecturer's context
                else
                {
                    System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("ID") == null);
                    //checks FOR STUDENTS WHO ARE MESSING AROUND WITH THE QUERY STRING, this checks whether the logged in lecturerID is NULL
                    Student lecturerStudent = studentContext.GetStudentDetails(id.Value);
                    //checks whether the student ID exists
                    if (lecturerStudent == null)
                    {
                        TempData["ErrorMessage"] = "Student Does Not Exist!";
                        return RedirectToAction("Mentee", "Lecturer");
                    }
                    StudentViewModel StudentVM = MapToLecturer(lecturerStudent);
                    List<StudentSkillSetViewModel> allskillsetList = studentskillsetContext.GetAllSkillSets();
                    allskillsetList = studentskillsetContext.GetStudentsSkillSet(StudentVM.StudentID);
                    ViewBag.list = allskillsetList;
                    //checks whether the student's mentor ID matches with the logged in lecturer's ID
                    //if (lecturerStudent.MentorID != Convert.ToInt32(HttpContext.Session.GetString("ID")))
                    //{
                    //    TempData["ErrorMessage"] = "You are not allowed to view students that are not your Mentees!";
                    //    return RedirectToAction("Mentee", "Lecturer");
                    //}
                    return View(StudentVM);
                    //else
                    //{
                    //    int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
                    //    Student student = studentContext.GetStudentDetails(studentid);
                    //    StudentViewModel StudentVM = MapToLecturer(student);
                    //    List<StudentSkillSetViewModel> allskillsetList = studentskillsetContext.GetAllSkillSets();
                    //    allskillsetList = studentskillsetContext.GetStudentsSkillSet(studentid);
                    //    ViewBag.list = allskillsetList;
                    //    ViewData["ErrorMessage"] = "You Are Not Allowed To View Other Student's Details!";
                    //    return View(StudentVM);
                    //}
                }
            }
            return RedirectToAction("Index", "Home");
        }
        //Updating student portfolio page
        public ActionResult Update(int id)
        {
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            // Stop accessing the action if not logged in
            // or account not in the "Student" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            //System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            Student student = studentContext.GetStudentDetails(studentid);
            ViewData["CourseSelect"] = GetCourse();
            ViewData["LecturerSelect"] = GetLecturer();
            //List<StudentViewModel> studentVMList = new List<StudentViewModel>();
            //StudentViewModel studentVM = MapToLecturer(student);
            //studentVMList.Add(studentVM);
            return View(student);
        }
        //Button on updating student portfolio page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Student student)
        {
            if (ModelState.IsValid)
            {
                ViewData["CourseSelect"] = GetCourse();
                ViewData["LecturerSelect"] = GetLecturer();
                studentContext.UpdateProfile(student);
                ViewData["Message"] = "Student profile updated Successfully!";
                return View("Update");
            }
            ViewData["CourseSelect"] = GetCourse();
            ViewData["LecturerSelect"] = GetLecturer();
            return View("Update");
        }

        public StudentViewModel MapToLecturer(Student student)
        {
            string lecturername = "";
            List<Lecturer> lecturerList = lecturerContext.GetAllLecturer();
            foreach (Lecturer lecturer in lecturerList)
            {
                if (lecturer.LecturerId == student.MentorID)
                {
                    lecturername = lecturer.Name;
                }
            }
            StudentViewModel studentVM = new StudentViewModel
            {
                StudentID = student.StudentID,
                Name = student.Name,
                Course = student.Course,
                Photo = student.Photo,
                Description = student.Description,
                Achievement = student.Achievement,
                ExternalLink = student.ExternalLink,
                EmailAddr = student.EmailAddr,
                Password = student.Password,
                MentorID = student.MentorID,
                LecturerName = lecturername,
            };
            return studentVM;
        }
        //Uploading student profile photo
        public ActionResult UpdatePhoto()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Student" role in
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            StudentPhoto studentPhoto = studentContext.GetPhotoDetails(studentid);
            return View(studentPhoto);
        }
        //Button on updating student profile photo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhoto(StudentPhoto student)
        {
            if (student.FileToUpload != null && student.FileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(student.FileToUpload.FileName);
                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = student.StudentID + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(savePath, FileMode.Create))
                    {
                        await student.FileToUpload.CopyToAsync(fileSteam);
                    }
                    student.Photo = uploadedFile;
                    studentContext.UpdatePhoto(student);
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
        //Updating skill set page
        public ActionResult UpdateSkillSet()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Student" role in
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            CheckBoxList options = new CheckBoxList();
            List<StudentSkillSetViewModel> allskillsetList = studentskillsetContext.GetAllSkillSets();
            List<StudentSkillSetViewModel> studentskillsetList = new List<StudentSkillSetViewModel>();
            foreach (StudentSkillSetViewModel skillset in allskillsetList)
            {
                bool check = studentskillsetContext.CheckStudentSkillSets(skillset.SkillSetID, studentid);
                studentskillsetList.Add(
                new StudentSkillSetViewModel
                {
                    StudentID = studentid,
                    SkillSetID = Convert.ToInt32(skillset.SkillSetID),
                    SkillSetName = skillset.SkillSetName,
                    IsChecked = check
                });
            }
            options.CheckBoxOptions = studentskillsetList;
            return View(options);
        }
        //Updating student skill set page button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSkillset(CheckBoxList checkboxes)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Student" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            CheckBoxList options = new CheckBoxList();
            List<StudentSkillSetViewModel> skillsetList = studentskillsetContext.GetAllSkillSets();
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            studentskillsetContext.DeleteSkillSets(studentid);
            foreach (var skillset in checkboxes.CheckBoxOptions)
            {
                if (skillset.IsChecked == true)
                {
                    studentskillsetContext.UpdateSkillSets(studentid, skillset.SkillSetID);
                }
            }
            List<StudentSkillSetViewModel> SkillSets = new List<StudentSkillSetViewModel>();
            foreach (StudentSkillSetViewModel skillset in skillsetList)
            {
                bool student = studentskillsetContext.CheckStudentSkillSets(skillset.SkillSetID, studentid);
                SkillSets.Add(
                new StudentSkillSetViewModel
                {
                    StudentID = studentid,
                    SkillSetID = Convert.ToInt32(skillset.SkillSetID),
                    SkillSetName = skillset.SkillSetName,
                    IsChecked = student
                });
            }
            options.CheckBoxOptions = SkillSets;
            ViewData["Message"] = "Skillsets successfully updated";
            return View(options);
        }
        //Change Password Page
        //GET: Student/UpdatePassword Function
        public ActionResult UpdatePassword()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
               (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            //set a variable from the session string logged in Student's ID
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            //get all the student's details based on the ID
            StudentUpdatePassword student = new StudentUpdatePassword
            {
                StudentID = studentid
            };
            return View(student);
        }
        //Update Password button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(StudentUpdatePassword student)
        {
            if (student.Password == null)
            {
                return View(student);
            }
            //Get password details for currently logged in Student
            StudentUpdatePassword currentStudent = studentContext.GetPassword(Convert.ToInt32(HttpContext.Session.GetInt32("StudentID")));
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(student.Password));
            string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            //if password DOES NOT match the database password...
            if (hashedPassword != currentStudent.Password)
            {
                ViewData["Message"] = "Current Password Is Incorrect!";
                return View(student);
            }
            //else continue what is needed to be done
            if (ModelState.IsValid)
            {
                //checks whether the password is the same
                if (student.NewPassword == student.ConfirmPassword)
                {
                    //Checks the password whether it contains a digit, hashes the password using SHA-1 and updates the password into the database
                    if (studentContext.ChangePassword(student))
                    {
                        ViewData["Message"] = "Password Changed Successfully!";
                        return View(student);
                    }
                }
                //if password does not match
                else
                {
                    ViewData["Message"] = "Password Does Not Match!";
                    return View(student);
                }
            }
            //if password field is empty OR does not match the required model from Lecturer.cs, return to view with error message
            ViewData["Message"] = "Password Field Did Not Meet Requirements!";
            return View(student);
        }
    }
}