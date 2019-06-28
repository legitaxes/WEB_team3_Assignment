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
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: SkillSet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SkillSet skillSet)
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SkillSet/Edit/5
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

        // GET: SkillSet/Delete/5
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
    