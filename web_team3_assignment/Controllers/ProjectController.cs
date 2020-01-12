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
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace web_team3_assignment.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectDAL projectContext = new ProjectDAL();
        private ProjectMemberDAL projectMemberContext = new ProjectMemberDAL();
        private StudentDAL studentContext = new StudentDAL();
        private HomeDAL homeContext = new HomeDAL();

        // GET: Project
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }
            ProjectMember projectmember = homeContext.getProjectID(HttpContext.Session.GetInt32("StudentID").Value);
            if (projectmember.Role != null || projectmember.ProjectId != 0)
            {
                HttpContext.Session.SetInt32("ProjectID", projectmember.ProjectId);
                HttpContext.Session.SetString("ProjectRole", projectmember.Role);
                List<Project> ProjectList = projectContext.GetAllProject(HttpContext.Session.GetInt32("StudentID"));
                System.Diagnostics.Debug.WriteLine(HttpContext.Session.GetString("ProjectRole"));


                //projectMemberList = projectContext method will getprojectmemberdetails by studentID in integer
                List<ProjectMember> projectMemberList = projectContext.GetProjectMemberDetails(HttpContext.Session.GetInt32("StudentID"));

                //Use ViewData to get ProjectList to view projectmemberList on _ViewProject.cshtml
                ViewData["ProjectList"] = projectMemberList;

                return View(ProjectList);
            }
            else
            {
                return View();
            }
        }

        //GET: Project/Create
        public ActionResult CreateProject()
        {
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
            //Check if there are any validation errors during model binding.
            //If validation is valid, Add project record to database
            //Else return view back to project

            if (ModelState.IsValid)
            {
                ProjectMember projectMember = new ProjectMember();

                //Add project record to database
                project.ProjectId = projectContext.Add(project);

                //set the property values for the projectmember to be prepared to insert into the database
                projectMember.ProjectId = project.ProjectId;

                //get the values for projectMember's studentID and set it to integer value
                projectMember.StudentId = HttpContext.Session.GetInt32("StudentID").Value;

                //projectmember role is = to  Leader
                projectMember.Role = "Leader";

                //projectContext method will add project as leader in projectmember
                projectContext.AddProjectAsLeader(projectMember);

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
            //project.ProjectMemberList = projectContext.GetAllProjectMM(id.Value);
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
            //System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                //Update staff record to database
                projectContext.Update(project);
                return RedirectToAction("Index");
            }

            return View(project);
        }


        // GET: Project/Details/5
        public ActionResult DetailProject(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Student"))
            {
                return RedirectToAction("Index", "Home");
            }

            //use projectContext method to getprojectdetails by parameter id
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

                //foreach projectmember in projectMemberList
                foreach (ProjectMember projectMember in projectMemberList)
                {
                    //if projectMember's projectID is equal to project's projectID
                    if (projectMember.ProjectId == project.ProjectId)
                    {
                        //Role will be equal to projectMember's role
                        Role = projectMember.Role;

                        //Exit the foreach loop once the role is found
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


        //// GET: Project/DeleteProject/5
        //public ActionResult DeleteProject(int? id)
        //{
        //    // Stop accessing the action if not logged in
        //    // or account not in the "Student" role
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //    (HttpContext.Session.GetString("Role") != "Student"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    if (id == null)
        //    {
        //        //Return to listing page, not allowed to edit
        //        return RedirectToAction("Index");
        //    }
        //    Project project = projectContext.GetProjectDetails(id.Value);
    
        //    return View(project);
        //}

        ////POST: Project/DeleteProject/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteProject(Project project)
        //{
        //    // Delete the project record from database
        //    projectContext.Delete(project.ProjectId);

        //    // Call the Index action of Home controller
        //    return RedirectToAction("Index");
        //}


        public ActionResult UploadPhoto(int id)
        {       
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

                    // Rename the uploaded file with the projectposter’s filename.
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

                //catch exception that is thrown in when an I/O error occurs
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