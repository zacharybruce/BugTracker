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

        // GET: Bugs/Details/5
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
            //ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName");
            return View();
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BugName,Priority,BugDescription,ProjectID")] Bug bug, string id)
        {
            if (ModelState.IsValid)
            {
                //var userID = User.Identity.GetUserName();
                bug.ProjectID = db.Projects.Where(x => x.ProjectName == id).Select(x => x.ID).Single();
                db.Bugs.Add(bug);
                db.SaveChanges();
                return RedirectToAction("Bugs", "Projects", new { id = id });
            }

            //ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", bug.ProjectID);
            return View(bug);
        }

        // GET: Bugs/Edit/5
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
            //ViewBag.Priority = new SelectList(db.Projects, "ID", "ProjectName", bug.ProjectID);
            return View(bug);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Bugs", "Projects", new { id = currentBug.ProjectID });
            }
            //ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", bug.ProjectID);
            return View(bug);
        }

        // GET: Bugs/Delete/5
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

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bug bug = db.Bugs.Find(id);
            db.Bugs.Remove(bug);
            db.SaveChanges();
            return RedirectToAction("Index", "Projects");
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
