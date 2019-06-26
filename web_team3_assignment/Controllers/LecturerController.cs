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
    public class LecturerController : Controller
    {
        private LecturerDAL lecturerContext = new LecturerDAL();
        // GET: Lecturer
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in 
            // or account not in the "Lecturer" role
            if ((HttpContext.Session.GetString("Role") == null) || 
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Lecturer> lecturerList = lecturerContext.GetAllLecturer();
            return View(lecturerList);
        }

        // GET: Lecturer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Lecturer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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