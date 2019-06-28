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
            List<Project> ProjectList = projectContext.GetAllProject();
            return View(ProjectList);     
        }

        // GET: ProjectMembers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: ProjectMember/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                project.ProjectId = projectContext.Add(project);

                //Redirect user to Project/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view 
                //to display error message
            }
                return View(project);

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

        public ActionResult CreateProject()
        {
            return View();
        }

        // GET: Lecturer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Lecturer/Edit/5
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

        // GET: Lecturer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Lecturer/Delete/5
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