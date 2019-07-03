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
    public class ProjectMemberController : Controller
    {
        private ProjectDAL projectContext = new ProjectDAL();
        private ProjectMemberDAL projectMemberContext = new ProjectMemberDAL();
        private StudentDAL studentContext = new StudentDAL();

        public ActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<ProjectMember> projectMemberList = projectMemberContext.GetAllProjectMember();
            return View(projectMemberList);
        }

        //public ActionResult Index()
        //{
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //    (HttpContext.Session.GetString("Role") != "Student"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    List<SuggestionViewModel> suggestionVMList = new List<SuggestionViewModel>();
        //    List<Suggestion> suggestionList = suggestionContext.GetSuggestionPostedByMentor(Convert.ToInt32(HttpContext.Session.GetString("ID")));
        //    foreach (Suggestion suggestion in suggestionList)
        //    {
        //        SuggestionViewModel suggestionVM = MapToStudentVM(suggestion);
        //        suggestionVMList.Add(suggestionVM);
        //    }
        //    return View(suggestionVMList);
        //}
    }
}