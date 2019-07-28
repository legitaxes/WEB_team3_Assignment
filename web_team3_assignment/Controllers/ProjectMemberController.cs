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
    }
}