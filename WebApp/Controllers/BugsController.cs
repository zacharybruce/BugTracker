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
        public ActionResult Index()
        {
            var bugs = db.Bugs.Include(b => b.Project);
            return View(bugs.ToList());
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
    }
}
