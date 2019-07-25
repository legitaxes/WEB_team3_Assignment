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
    public class SuggestionController : Controller
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        private SuggestionDAL suggestionContext = new SuggestionDAL();
        private StudentDAL studentContext = new StudentDAL();

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

        // GET: Suggestion
        public ActionResult StudentSuggestion()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<StudentSuggestionViewModel> studentsuggestionVMList = new List<StudentSuggestionViewModel>();
            List<Suggestion> suggestionList = suggestionContext.GetSuggestionPostedByStudentsMentor(Convert.ToInt32(HttpContext.Session.GetInt32("StudentID")));
            foreach (Suggestion suggestion in suggestionList)
            {
                StudentSuggestionViewModel suggestionVM = MapToLecturerVM(suggestion);
                studentsuggestionVMList.Add(suggestionVM);
            }
            return View(studentsuggestionVMList);
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
                return RedirectToAction("Index");
            }
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            StudentSuggestionViewModel suggestionVM = MapToLecturerVM(suggestion);
            if (suggestion == null)
            {
                return RedirectToAction("StudentSuggestion");
            }
            return View(suggestionVM);
        }

        // POST: Suggestion/Acknowledge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acknowledge(Suggestion suggestion)
        {
            // Delete the staff record from database
            suggestionContext.Acknowledge(suggestion.SuggestionId);
            return RedirectToAction("StudentSuggestion");
        }

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
            ViewData["MenteeList"] = lecturerContext.GetMentees(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            suggest.LecturerId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            suggest.Status = 'N';
            System.Diagnostics.Debug.WriteLine(suggest.StudentId);
            System.Diagnostics.Debug.WriteLine(suggest.Description);
            System.Diagnostics.Debug.WriteLine(suggest.LecturerId);
            System.Diagnostics.Debug.WriteLine(suggest.Status);
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
            //foreach (var item in ViewData["Status"] as List<SelectListItem>)
            //{
            //    if (item.Value == suggestion.Status.ToString())
            //    {
            //        item.Selected = true;
            //        break;
            //    }
            //}
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
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            //suggestionVM.SelectedStatusItem = GetSuggestionStatus(id.Value);
            return View(suggestionVM);
        }

        // POST: Suggestion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suggestion suggestion)
        {
            ViewData["Status"] = GetSuggestionStatus(suggestion.SuggestionId);
            if (suggestion.Description == null || suggestion.Description == " ")
            {
                ViewData["Message"] = "Description Field Cannot be Empty!";
                SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
                return View(suggestionVM);
            }
            if (ModelState.IsValid)
            {
                suggestionContext.Update(suggestion);
                return RedirectToAction("Index");
            }
            return View(suggestion);
        }

        // GET: Suggestion/Delete/5
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
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            //check if suggestion exists
            if (suggestion == null)
            {
                TempData["Message"] = "The Suggestion does not exist!";
                return RedirectToAction("Index");
            }
            //check if it's lecturer's suggestion
            if (suggestion.LecturerId != Convert.ToInt32(HttpContext.Session.GetString("ID")))
            {
                TempData["Message"] = "You are not allowed to delete other lecturer's suggestion!";
                return RedirectToAction("Index");
            }

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

        //get a selectlistitem for status 
        private List<SelectListItem> GetSuggestionStatus(int id)
        {
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id);
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(
                new SelectListItem
                {
                    Value = "Y",
                    Text = "Acknowledged",
                    Selected = suggestion.Status == 'Y' ? true : false
                });
            status.Add(
                new SelectListItem
                {
                    Value = "N",
                    Text = "Not Acknowledged",
                    Selected = suggestion.Status == 'N' ? true : false
                });
            return status;
        }
    }
}