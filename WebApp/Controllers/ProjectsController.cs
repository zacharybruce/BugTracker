﻿using Microsoft.AspNet.Identity;
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
        public ActionResult Create(string id)
        {
            if (id != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Email");
            return View();
        }

        // POST: Projects/Create
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

            return View(project);
        }

        // POST: Projects/Edit/5
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

        // POST: Projects/Delete
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
    }
}
