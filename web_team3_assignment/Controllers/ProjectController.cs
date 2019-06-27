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
    public class ProjectController : Controller
    {
        private ProjectDAL projectContext = new ProjectDAL();

        // GET: Project
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<ProjectMember> ProjectMemberList = projectContext.GetAllProjectMember();
            return View(ProjectMemberList);     
        }
    }
}