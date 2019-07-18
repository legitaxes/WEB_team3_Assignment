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


        public ActionResult Index(int? id)
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            ProjectMemberViewModel projectMemberVM = new ProjectMemberViewModel();
            projectMemberVM.projectMemberList = projectMemberContext.GetAllProjectMembers();

            // BranchNo (id) present in the query string
            if (id != null)
            {
                ViewData["selectedProjectId"] = id.Value;
                projectMemberVM.projectList = projectMemberContext.GetProjectPM(id.Value);
            }
            else
            {
                ViewData["selectedProjectId"] = "";
            }
            return View(projectMemberVM);

            //return View();
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


        // GET: Suggestion/Details/5
        //public ActionResult DetailProject(int id)
        //{

        //    // Stop accessing the action if not logged in 
        //    // or account not in the "Staff" role
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //        (HttpContext.Session.GetString("Role") != "Student"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View();

        //    //ProjectMember projectMember = projectMemberContext.GetProjectMemberDetails(id);
        //    //ProjectMemberViewModel projectMemberVM = MapToProject(projectMember);

        //    //return View(projectMemberVM);

        //}

        //public ProjectMemberViewModel MapToProjectVM(ProjectMemberViewModel projectMember)
        //{
        //    if (projectMember != null)
        //    {


        //        string Role = "";
        //        List<Project> projectList = projectContext.GetAllProject();
        //        foreach (Project project in projectList)
        //        {
        //            if (project.ProjectId == projectMember.ProjectId)
        //            {
        //                Role = projectMember.Role;
        //                break;
        //            }
        //        }


        //        ProjectMemberViewModel projectMemberVM = new ProjectMemberViewModel
        //        {
        //            ProjectId = projectMember.ProjectId,
        //            StudentId = projectMember.ProjectId,
        //            Role = projectMember.Role,
        //            Title = projectMember.Title,
        //            ProjectPoster = projectMember.ProjectPoster,
        //            ProjectURL = projectMember.ProjectURL,
        //            Description = projectMember.Description
        //        };
        //        return projectMemberVM;
        //    }
        //    else
        //        return null;
        //}
    }
}