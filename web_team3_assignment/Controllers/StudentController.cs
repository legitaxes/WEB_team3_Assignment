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
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SkillSetDAL skillsetContext = new SkillSetDAL();
        private StudentSkillSetDAL studentskillsetContext = new StudentSkillSetDAL();

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
            //System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            Student student = studentContext.GetStudentDetails(studentid);
            List<StudentViewModel> studentVMList = new List<StudentViewModel>();
            StudentViewModel studentVM = MapToLecturer(student);
            studentVMList.Add(studentVM);
            return View(studentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                studentContext.UpdateProfile(student);
                ViewData["Message"] = "Student profile updated Successfully!";
                return View("Update");
            }
            return View("Update");
        }

        public StudentViewModel MapToLecturer(Student student)
        {
            string lecturername = "";
            List<SkillSet> skillsetList = skillsetContext.GetAllSkillSet();
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

        public ActionResult UpdatePhoto()
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

        public ActionResult UpdateSkillSet()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role in
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSkillset(CheckBoxList checkboxes)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
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
    }
}