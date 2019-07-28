using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SuggestionController : Controller
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SuggestionDAL suggestionContext = new SuggestionDAL();
        private StudentDAL studentContext = new StudentDAL();
        // View All Suggestion Posted by the Currently Logged In Lecturer
        // GET: Suggestion
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<SuggestionViewModel> suggestionVMList = new List<SuggestionViewModel>();
            List<Suggestion> suggestionList = suggestionContext.GetSuggestionPostedByMentor(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            foreach(Suggestion suggestion in suggestionList)
            {
                SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
                suggestionVMList.Add(suggestionVM);
            }
            return View(suggestionVMList);
        }
   
        public SuggestionViewModel MapToStudentVM(Suggestion suggestion)
        {
            if (suggestion != null)
            {
                string studentName = "";
                List<Student> studentList = studentContext.GetAllStudent();
                foreach (Student student in studentList)
                {
                    if (student.StudentID == suggestion.StudentId)
                    {
                        studentName = student.Name;
                        break;
                    }
                }
                string suggestionStatus;
                if (suggestion.Status == 'N')
                {
                    suggestionStatus = "Not Acknowledged";
                }
                else
                {
                    suggestionStatus = "Acknowledged";
                }
                SuggestionViewModel suggestionVM = new SuggestionViewModel
                {
                    SuggestionId = suggestion.SuggestionId,
                    LecturerId = suggestion.LecturerId,
                    Description = suggestion.Description,
                    Status = suggestionStatus,
                    DateCreated = suggestion.DateCreated,
                    StudentName = studentName
                };
                return suggestionVM;
            }
            else
                return null;
        }

        public StudentSuggestionViewModel MapToLecturerVM(Suggestion suggestion)
        {
            if (suggestion != null)
            {
                string lecturerName = "";
                List<Lecturer> lecturerList = lecturerContext.GetAllLecturer();
                Student student = studentContext.GetStudentDetails(Convert.ToInt32(HttpContext.Session.GetInt32("StudentID")));
                foreach (Lecturer lecturer in lecturerList)
                {
                    if (lecturer.LecturerId == suggestion.LecturerId && lecturer.LecturerId == student.MentorID && student.StudentID == suggestion.StudentId)
                    {
                        lecturerName = lecturer.Name;
                        break;
                    }
                }
                string suggestionStatus;
                if (suggestion.Status == 'N')
                {
                    suggestionStatus = "Not Acknowledged";
                }
                else
                {
                    suggestionStatus = "Acknowledged";
                }
                StudentSuggestionViewModel studentVM = new StudentSuggestionViewModel
                {
                    SuggestionId = suggestion.SuggestionId,
                    LecturerId = suggestion.LecturerId,
                    StudentId = suggestion.StudentId,
                    Description = suggestion.Description,
                    Status = suggestionStatus,
                    DateCreated = suggestion.DateCreated,
                    LecturerName = lecturerName
                };
                return studentVM;
            }
            else
                return null;
        }

        // GET: Suggestion
        public ActionResult StudentSuggestion()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            List<StudentSuggestionViewModel> studentsuggestionVMList = new List<StudentSuggestionViewModel>();
            List<Suggestion> suggestionList = suggestionContext.GetSuggestionPostedByStudentsMentor(Convert.ToInt32(HttpContext.Session.GetInt32("StudentID")));
            foreach (Suggestion suggestion in suggestionList)
            {
                StudentSuggestionViewModel suggestionVM = MapToLecturerVM(suggestion);
                studentsuggestionVMList.Add(suggestionVM);
            }
            return View(studentsuggestionVMList);
        }

        // GET: Suggestion/Acknowledge/5
        public ActionResult Acknowledge(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                TempData["ErrorMessage"] = "You are not allowed to acknowledge other student's suggestions!";
                return RedirectToAction("Index");
            }
            int studentid = Convert.ToInt32(HttpContext.Session.GetInt32("StudentID"));
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            StudentSuggestionViewModel suggestionVM = MapToLecturerVM(suggestion);
            if (suggestion == null || suggestion.StudentId != studentid)
            {
                TempData["ErrorMessage"] = "You are not allowed to acknowledge other student's suggestions!";
                return RedirectToAction("StudentSuggestion");
            }
            return View(suggestionVM);
        }

        // POST: Suggestion/Acknowledge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acknowledge(Suggestion suggestion)
        {
            // Acknowledge the Suggestion using a method called Acknowledge
            suggestionContext.Acknowledge(suggestion.SuggestionId);
            return RedirectToAction("StudentSuggestion");
        }
        
        // Post Suggestion Page
        // GET: Suggestion/PostSuggestion
        public ActionResult PostSuggestion()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["MenteeList"] = lecturerContext.GetMentees(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            return View();
        }

        // POST: Suggestion/PostSuggestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostSuggestion(Suggestion suggest)
        {
            int lecturerid = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            ViewData["MenteeList"] = lecturerContext.GetMentees(lecturerid);
            suggest.LecturerId = lecturerid;
            suggest.Status = 'N';
            if (ModelState.IsValid)
            {
                suggest.SuggestionId = suggestionContext.PostSuggestion(suggest);
                ViewData["Message"] = "Suggestion Posted Successfully";
                return View();
            }
            else
            {
                return View(suggest);
            }
        }
        // Edit Suggestion Page
        // GET: Suggestion/Edit/5
        public ActionResult Edit(int? id)
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

            //gets the suggestion information based on the URL ID
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            //if the suggestion cannot be found, return null and return a message to the index action
            if (suggestion == null)
            {
                TempData["Message"] = "The Suggestion does not exist!";
                return RedirectToAction("Index");
            }
            //if the suggestion's lecturer id does not match with the logged in lecturer's ID
            if (suggestion.LecturerId != Convert.ToInt32(HttpContext.Session.GetString("ID")))
            {
                TempData["Message"] = "You are not allowed to edit other lecturer's suggestion!";
                return RedirectToAction("Index");
            }
            //if all validations are ok, return the page with the suggestion mapped properly with proper fields
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            return View(suggestionVM);
        }

        // POST: Suggestion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suggestion suggestion)
        {
            //check whether description is null or 'empty' string
            if (suggestion.Description == null || suggestion.Description == " ")
            {
                ViewData["Message"] = "Description Field Cannot be Empty!";
                SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
                return View(suggestionVM);
            }
            //if everything is ok, update the suggestion
            if (ModelState.IsValid)
            {
                suggestionContext.Update(suggestion);
                return RedirectToAction("Index");
            }
            return View(suggestion);
        }
        // Delete Suggestion Page
        // GET: Suggestion/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            //if the query string is empty, return to the index
            if (id == null)
            {
                TempData["Message"] = "Please Select A Suggestion To Delete";
                return RedirectToAction("Index");
            }
            //retrieve all the suggestion details
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            //check whether suggestion exists
            if (suggestion == null)
            {
                TempData["Message"] = "The Suggestion does not exist!";
                return RedirectToAction("Index");
            }
            //check if it's NOT the lecturer's suggestion
            if (suggestion.LecturerId != Convert.ToInt32(HttpContext.Session.GetString("ID")))
            {
                TempData["Message"] = "You are not allowed to delete other lecturer's suggestion!";
                return RedirectToAction("Index");
            }
            //map the suggestion to proper fields
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            return View(suggestionVM);
        }

        // POST: Suggestion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Suggestion suggestion)
        {
            // Delete the staff record from database
            suggestionContext.Delete(suggestion.SuggestionId);
            return RedirectToAction("Index");
        }
    }
}