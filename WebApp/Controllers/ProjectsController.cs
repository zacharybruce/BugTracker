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
    public class ProjectsController : Controller
    {
        private BugTrackerContext db = new BugTrackerContext();

        // GET: Projects
        public ActionResult Index()
        {
            string currentUser = User.Identity.GetUserName();

            var projects = db.Projects.Include(p => p.Profile);
            var userProjects = projects.Where(p => p.Profile.Email == currentUser);

            return View(userProjects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            //ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectName,ProjectDescription,ProfileID")] Project project)
        {
            if (ModelState.IsValid)
            {
                var userID = User.Identity.GetUserName();
                project.ProfileID = db.Profiles.Where(x => x.Email == userID).Select(x => x.ID).Single();
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email", project.ProfileID);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email", project.ProfileID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                var currentProject = db.Projects.FirstOrDefault(p => p.ID == project.ID);
                currentProject.ProjectName = project.ProjectName;
                currentProject.ProjectDescription = project.ProjectDescription;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email", project.ProfileID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Bugs(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();
            return View(allBugs);
        }

        public ActionResult LowPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var lowBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Low").ToList();
            return View("Bugs", lowBugs);
        }

        public ActionResult NormalPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var normalBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Normal").ToList();
            return View("Bugs", normalBugs);
        }

        public ActionResult HighPriority(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var highBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "High").ToList();
            return View("Bugs", highBugs);
        }

        public ActionResult ImmediatePriority(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var immediateBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == "Immediate").ToList();
            return View("Bugs", immediateBugs);
        }

        public ActionResult SortByRecent(string id)
        {
            string currentUser = User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);

            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();
            allBugs.Reverse();

            return View("Bugs", allBugs);
        }

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
            return View("Bugs", sortedByPriority);
        }
    }
}
