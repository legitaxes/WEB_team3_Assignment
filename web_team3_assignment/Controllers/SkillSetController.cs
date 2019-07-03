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
    public class SkillSetController : Controller
    {
        private SkillSetDAL SkillSetContext = new SkillSetDAL();
        public IActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<SkillSet> SkillSetList = SkillSetContext.GetAllSkillSet();
            return View(SkillSetList);
        }

        public ActionResult SkillSetCreate()
        {
            return View();
        }

        // GET: SkillSet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SkillSet/Create
        public ActionResult Create(SkillSet skillset)
        {
            // Stop accessing the action if not logged in // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            SkillSetContext.Add(skillset);
            return RedirectToAction("Index", "SkillSet");
            return View();
        }

        // POST: SkillSet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SkillSetCreate(SkillSet skillSet)
        {
            if (ModelState.IsValid)
            {
                skillSet.SkillSetId = SkillSetContext.Add(skillSet);
                return RedirectToAction("Index");
            }
            else
            {

                return View(skillSet);
            }

        }

        // GET: SkillSet/Edit/5
        public ActionResult SkillSetEdit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null) //Query string parameter not provided
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            SkillSet skillset = SkillSetContext.GetDetails(id.Value);
            if (skillset == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(skillset);
        }

        // POST: SkillSet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SkillSet skillset)
        {
            //Get branch list for drop-down list
            //in case of the need to return to Edit.cshtml view
            if (ModelState.IsValid)
            {
                //Update skillset record to database
                SkillSetContext.Update(skillset);
                return RedirectToAction("Index");
            }
            //Input validation fails, return to the view
            //to display error message
            return View(skillset);
        }

        // GET: Lecturer/Delete/5
        public ActionResult SkillSetDelete(int? id)
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
            SkillSet skillSet = SkillSetContext.GetDetails(id.Value);

            return View(skillSet);
        }

        // POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SkillSetDelete(SkillSet skillSet)
        {
            // Delete the staff record from database
            SkillSetContext.Delete(skillSet.SkillSetId);
            // Call the Index action of Home controller
            return RedirectToAction("Index", "Home");

        }
    }
}
    