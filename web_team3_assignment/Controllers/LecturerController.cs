﻿using System;
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

        //Done
        // GET: Lecturer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //Done
        // GET: Lecturer/Create
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

        //Done

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lecturer lecturer)
        {
            lecturer.Password = "p@55Mentor";
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Employee Created Successfully";
                lecturer.LecturerId = lecturerContext.Add(lecturer);
                return View();
            }
            else
            {
                return View(lecturer);
            }
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

        // GET: Lecturer/PostSuggestion
        public ActionResult PostSuggestion()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Lecturer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["MenteeList"] = lecturerContext.GetMentees(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Lecturer/PostSuggestion
        public ActionResult PostSuggestion(Suggestion suggest)
        {
            //ViewData["MenteeList"] = lecturerContext.GetMentees(Convert.ToInt32(HttpContext.Session.GetString("ID")));
            suggest.LecturerId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            suggest.Status = 'N';
            System.Diagnostics.Debug.WriteLine(suggest.StudentId);
            System.Diagnostics.Debug.WriteLine(suggest.Description);
            System.Diagnostics.Debug.WriteLine(suggest.LecturerId);
            System.Diagnostics.Debug.WriteLine(suggest.Status);
            if (ModelState.IsValid)
            {
                suggest.SuggestionId = lecturerContext.PostSuggestion(suggest);
                ViewData["Message"] = "Suggestion Posted Successfully";
                return View();
            }
            else
            {
                return View(suggest);
            }
        }
    }
}