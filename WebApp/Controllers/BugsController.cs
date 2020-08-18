using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.DataAccess;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class BugsController : Controller
    {
        private BugTrackerContext db = new BugTrackerContext();

        // GET: Bugs
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If project does not exist, throw 404 error
            var project = db.Projects.Where(p => p.ProjectName == id).ToList();
            if (project.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            List<Bug> allBugs = SqlLogic.GetBugsForCurrentProject(id).OrderBy(x => x.Status).ToList();

            return View(allBugs);
        }

        // GET: Bugs/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bug bug = SqlLogic.FindBug(id);

            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        // GET: Bugs/Create
        public ActionResult Create(string id)
        {
            return View();
        }

        // POST: Bugs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BugName,Priority,BugDescription,Status,ProjectID")] Bug bug, string id)
        {
            if (ModelState.IsValid)
            {
                SqlLogic.CreateBug(bug, id);
                return RedirectToAction("Index", "Bugs", new { id = id });
            }

            return View(bug);
        }

        // GET: Bugs/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bug bug = SqlLogic.FindBug(id);
            if (bug == null)
            {
                return HttpNotFound();
            }

            return View(bug);
        }

        // POST: Bugs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bug bug, int id)
        {
            if (ModelState.IsValid)
            {
                Bug currentBug = SqlLogic.GetCurrentBug(bug);
                SqlLogic.EditBug(bug, id);
                return RedirectToAction("Index", "Bugs", new { id = currentBug.Project.ProjectName });
            }
            return View(bug);
        }

        // GET: Bugs/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = SqlLogic.FindBug(id);
            if (bug == null)
            {
                return HttpNotFound();
            }
            return View(bug);
        }

        // POST: Bugs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bug bug = SqlLogic.FindBug(id);
            string currentProject = bug.Project.ProjectName;

            SqlLogic.DeleteBugConfirmation(bug);

            return RedirectToAction("Index", "Bugs", new { id = currentProject });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Returns only low priority bugs for current project
        public ActionResult LowPriority(string id)
        {
            List<Bug> lowBugs = SqlLogic.GetPriorityBugs("Low", id).OrderBy(x => x.Status).ToList();

            return View("Index", lowBugs);
        }

        // Returns only normal priority bugs for current project
        public ActionResult NormalPriority(string id)
        {
            List<Bug> normalBugs = SqlLogic.GetPriorityBugs("Normal", id).OrderBy(x => x.Status).ToList();

            return View("Index", normalBugs);
        }

        // Returns only high priority bugs for current project
        public ActionResult HighPriority(string id)
        {
            List<Bug> highBugs = SqlLogic.GetPriorityBugs("High", id).OrderBy(x => x.Status).ToList();

            return View("Index", highBugs);
        }

        // Returns only low priority bugs for current project
        public ActionResult ImmediatePriority(string id)
        {
            List<Bug> immediateBugs = SqlLogic.GetPriorityBugs("Immediate", id).OrderBy(x => x.Status).ToList();

            return View("Index", immediateBugs);
        }

        // Sorts bug list where most recent bugs are before older ones
        public ActionResult SortByRecent(string id)
        {
            List<Bug> allBugs = SqlLogic.SortBugsByRecent(id).OrderBy(x => x.Status).ToList();

            return View("Index", allBugs);
        }

        // Sorts bug list by priority.  Order is low, normal, high, then immediate
        public ActionResult SortByPriority(string id)
        {
            IOrderedEnumerable<Bug> sortedByPriority = SqlLogic.SortBugsByPriority(id);
            return View("Index", sortedByPriority);
        }
    }
}
