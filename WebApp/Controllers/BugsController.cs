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

            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);

            // If project does not exist, throw 404 error
            var project = db.Projects.Where(p => p.ProjectName == id).ToList();
            if (project.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();

            return View(allBugs);
        }

        // GET: Bugs/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bug bug = db.Bugs.Find(id);
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
        public ActionResult Create([Bind(Include = "BugName,Priority,BugDescription,ProjectID")] Bug bug, string id)
        {
            if (ModelState.IsValid)
            {
                string currentUser = User.Identity.GetUserName();
                bug.ProjectID = db.Projects.Where(x => x.ProjectName == id && x.Profile.Email == currentUser).Select(x => x.ID).Single();
                db.Bugs.Add(bug);
                db.SaveChanges();
                return RedirectToAction("Bugs", "Projects", new { id = id });
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

            Bug bug = db.Bugs.Find(id);
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
                var currentBug = db.Bugs.FirstOrDefault(b => b.ID == bug.ID);
                currentBug.BugName = bug.BugName;
                currentBug.BugDescription = bug.BugDescription;
                currentBug.Priority = bug.Priority;
                db.SaveChanges();
                return RedirectToAction("Bugs", "Projects", new { id = currentBug.Project.ProjectName });
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
            Bug bug = db.Bugs.Find(id);
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
            Bug bug = db.Bugs.Find(id);
            var currentProject = bug.Project.ProjectName;
            db.Bugs.Remove(bug);
            db.SaveChanges();
            return RedirectToAction("Bugs", "Projects", new { id = currentProject });
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
            string currentUser = User.Identity.GetUserName();

            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var lowBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Low").ToList();

            return View("Index", lowBugs);
        }

        // Returns only normal priority bugs for current project
        public ActionResult NormalPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();

            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var normalBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Normal").ToList();

            return View("Index", normalBugs);
        }

        // Returns only high priority bugs for current project
        public ActionResult HighPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();

            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var highBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "High").ToList();

            return View("Index", highBugs);
        }

        // Returns only low priority bugs for current project
        public ActionResult ImmediatePriority(string id)
        {
            string currentUser = User.Identity.GetUserName();

            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var immediateBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Immediate").ToList();

            return View("Index", immediateBugs);
        }

        // Sorts bug list where most recent bugs are before older ones
        public ActionResult SortByRecent(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);

            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();
            allBugs.Reverse();

            return View("Index", allBugs);
        }

        // Sorts bug list by priority.  Order is low, normal, high, then immediate
        public ActionResult SortByPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();

            var sortOrder = new Dictionary<string, int>
            {
                { "Low", 1 },
                { "Normal", 2 },
                { "High", 3 },
                { "Immediate", 4 }
            };

            var defaultOrder = sortOrder.Max(x => x.Value) + 1;

            var sortedByPriority = allBugs.OrderBy(p => sortOrder.TryGetValue(p.Priority.ToString(), out var order) ? order : defaultOrder);

            return View("Index", sortedByPriority);
        }
    }
}
