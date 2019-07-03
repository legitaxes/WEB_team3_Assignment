using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web_team3_assignment.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectDAL projectContext = new ProjectDAL();
        private ProjectMemberDAL projectMemberContext = new ProjectMemberDAL();

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
            List<Project> ProjectList = projectContext.GetAllProject();
            return View(ProjectList);     
        }


        public ActionResult CreateProject()
        {
            // Stop accessing the action if not logged in // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: ProjectMember/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                //Add project record to database
                project.ProjectId = projectContext.Add(project);

                //Redirect user to Project/Index view
                return RedirectToAction("Index");
            }
            else
            {
                return View(project);
            }
        }
        
        //public ActionResult EditProject(int? id)
        //{
        //    // Stop accessing the action if not logged in // or account not in the "Lecturer" role
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //        (HttpContext.Session.GetString("Role") != "Student"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    Project project = projectContext.GetProjectDetails(id.Value);
        //    return View(project);
        //}

        // GET: Lecturer/Edit/5
        public ActionResult EditProject(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) //Query string parameter not provided
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }


            Project project = projectContext.GetProjectDetails(id.Value);
            if (project == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(project);
        }

  

        // POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            //Get branch list for drop-down list
            //in case of the need to return to Edit.cshtml view
            if (ModelState.IsValid)
            {
                //Update staff record to database
                projectContext.Update(project);
                return RedirectToAction("Index");
            }
            //Input validation fails, return to the view
            //to display error message
            return View(project);
        }


        // GET: Suggestion/Details/5
        public ActionResult DetailsProject(int id)
        {

            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Project project = projectContext.GetProjectDetails(id);
            //ProjectViewModel projectVM = MapToProjectVM(project);

            //return View(projectVM);


            return View();

        }

        //public ProjectViewModel MapToProjectVM(Project project)
        //{
        //    string Role = "";
        //    if (project.Title != null)
        //    {
        //        List<ProjectMember> pmList = pmContext.GetAllpm();
        //        foreach (ProjectMember projectMember in pmList)
        //        {
        //            if (projectMember.ProjectId == project.ProjectId)
        //            {
        //                Role = projectMember.Role;
        //                //Exit the foreach loop once the name is found
        //                break;
        //            }
        //        }
        //    }
           

        //    ProjectViewModel projectVM = new ProjectViewModel
        //    {
        //        ProjectId = project.ProjectId,
        //        Title = project.Title,
        //        ProjectURL = project.ProjectURL,
        //        Description = project.Description,
        //        ProjectPoster = project.ProjectPoster + ".jpg"
        //    };
        //    return projectVM;
        //}



        //// GET: ProjectMembers/Details/5
        //public ActionResult Details(int id)
        //{
        //    // Stop accessing the action if not logged in
        //    // or account not in the "Staff" role
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //    (HttpContext.Session.GetString("Role") != "Student"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View();

        //    //Project project = projectContext.GetProjectDetails(id);
        //    //ProjectViewModel projectVM = MapToProjectVM(project);
        //    //return View(projectVM);
        //}


        // GET: Lecturer/Delete/5
        public ActionResult DeleteProject(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Project project = projectContext.GetProjectDetails(id.Value);
            //if (project == null)
            //{
            //    //Return to listing page, not allowed to edit
            //    return RedirectToAction("Index");
            //}
            return View(project);
        }

        //POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(Project project)
        {
            // Delete the staff record from database
            projectContext.Delete(project.ProjectId);

            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }
    }
}