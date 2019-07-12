using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_team3_assignment.DAL;
using web_team3_assignment.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

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
        public ActionResult CreateProject(Project project)
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


        // GET: Lecturer/Edit/5
        public ActionResult EditProject(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Student" role
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



        // POST: Project/EditProject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(Project project)
        {
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                //Update staff record to database
                projectContext.Update(project);
                return RedirectToAction("Index");
            }

            //Input validation fails, return to the view
            ViewData["Message"] = "Update failed!";
            return View(project);
        }


        // GET: Project/Details/5
        public ActionResult DetailProject(int id)
        {

            // Stop accessing the action if not logged in 
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }


            Project project = projectContext.GetProjectDetails(id);
            ProjectViewModel projectVM = MapToProjectVM(project);

            return View(projectVM);




        }

        public ProjectViewModel MapToProjectVM(Project project)
        {
            string Role = "";
            if (project != null)
            {
                List<ProjectMember> projectMemberList = projectMemberContext.GetAllProjectMembers();
                foreach (ProjectMember projectMember in projectMemberList)
                {
                    if (projectMember.ProjectId == project.ProjectId)
                    {
                        Role = projectMember.Role;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }


            ProjectViewModel projectVM = new ProjectViewModel
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                ProjectURL = project.ProjectURL,
                Description = project.Description,
                projectphoto = project.ProjectPoster + ".jpg"
            };
            return projectVM;
        }


        // GET: Project/DeleteProject/5
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

        //POST: Project/DeleteProject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(Project project)
        {
            // Delete the staff record from database
            projectContext.Delete(project.ProjectId);

            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }


        public ActionResult UploadPhoto(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            Project project = projectContext.GetProjectDetails(id);
            ProjectViewModel projectVM = MapToProjectVM(project);
            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(ProjectViewModel projectVM)
        {
            if (projectVM.posterToUpload != null &&
            projectVM.posterToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                     projectVM.posterToUpload.FileName);

                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = projectVM.ProjectId + fileExt;

                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot\\images", uploadedFile);

                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await projectVM.posterToUpload.CopyToAsync(fileSteam);
                    }
                    projectVM.projectphoto = uploadedFile;
                    ViewData["Message"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(projectVM);
        }

    }
}