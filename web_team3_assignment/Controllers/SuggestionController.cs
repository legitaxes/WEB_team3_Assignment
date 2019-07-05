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
            List<StudentViewModel> studentsuggestionVMList = new List<StudentViewModel>();
            List<Suggestion> suggestionList = suggestionContext.GetSuggestionPostedByStudentsMentor(Convert.ToInt32(HttpContext.Session.GetInt32("StudentID")));
            foreach (Suggestion suggestion in suggestionList)
            {
                StudentViewModel suggestionVM = MapToLecturerVM(suggestion);
                studentsuggestionVMList.Add(suggestionVM);
            }
            return View(studentsuggestionVMList);
        }

        public StudentViewModel MapToLecturerVM(Suggestion suggestion)
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
                StudentViewModel studentVM = new StudentViewModel
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

        // GET: Suggestion/Delete/5
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
            StudentViewModel suggestionVM = MapToLecturerVM(suggestion);
            if (suggestion == null)
            {
                return RedirectToAction("StudentSuggestion");
            }
            return View(suggestionVM);
        }

        // POST: Suggestion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acknowledge(Suggestion suggestion)
        {
            // Delete the staff record from database
            suggestionContext.Acknowledge(suggestion.SuggestionId);
            return RedirectToAction("StudentSuggestion");
        }

        // GET: Suggestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

            ViewData["Status"] = GetSuggestionStatus();
            Suggestion suggestion = suggestionContext.GetSuggestionDetails(id.Value);
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            return View(suggestionVM);
        }

        // POST: Suggestion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suggestion suggestion)
        {
            ViewData["Status"] = GetSuggestionStatus();
            if (ModelState.IsValid)
            {
                suggestionContext.Update(suggestion);
                return RedirectToAction("Index");
            }
            ViewData["Message"] = "Description Field Cannot be Empty!";
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            return View(suggestionVM);
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
            SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
            if (suggestion == null)
            {
                return RedirectToAction("Index");
            }
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
        private List<SelectListItem> GetSuggestionStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(
                new SelectListItem
                {
                    Value = "Y",
                    Text = "Acknowledged"
                });
            status.Add(
                new SelectListItem
                {
                    Value = "N",
                    Text = "Not Acknowledged"
                });
            return status;
        }
    }
}