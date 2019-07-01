using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web_team3_assignment.Controllers
{
    public class SuggestionController : Controller
    {
        // GET: Suggestion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Suggestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Suggestion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suggestion/Create
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

        // GET: Suggestion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Suggestion/Edit/5
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

        // GET: Suggestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Suggestion/Delete/5
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