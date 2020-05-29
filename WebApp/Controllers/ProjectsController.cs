﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            var projects = db.Projects.Include(p => p.Profile);
            return View(projects.ToList());
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
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email", project.ProfileID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectName,ProjectDescription,ProfileID")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email", project.ProfileID);
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

        public ActionResult Bugs(int id)
        {
            var bugs = db.Bugs.Include(b => b.Project);
            return View(bugs.Where(b => b.ProjectID == id).ToList());
        }

        //public ActionResult NewBug(int id)
        //{
        //    //ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NewBug([Bind(Include = "ID,BugName,Priority,BugDescription,ProjectID")] Bug bug, int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var userID = User.Identity.GetUserName();
        //        bug.ProjectID = db.Projects.Where(x => x.ID == id).Select(x => x.ID).Single();
        //        db.Bugs.Add(bug);
        //        db.SaveChanges();
        //        return RedirectToAction("Bugs", "Projects", new { id = id});
        //    }

        //    ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", bug.ProjectID);
        //    return View(bug);
        //}

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
