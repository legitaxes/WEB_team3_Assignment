using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            List<Suggestion> suggestionList = suggestionContext.GetAllSuggestionDetails(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            foreach(Suggestion suggestion in suggestionList)
            {
                SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
                suggestionVMList.Add(suggestionVM);
            }
            return View(suggestionVMList);
        }

        // GET: Suggestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public SuggestionViewModel MapToStudentVM(Suggestion suggestion)
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Suggestion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Suggestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Suggestion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}