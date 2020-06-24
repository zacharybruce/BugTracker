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

namespace WebApp.DataAccess
{
    public class SqlLogic
    {
        private static BugTrackerContext db = new BugTrackerContext();
        public static List<Project> GetUserProjects()
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();

            var projects = db.Projects.Include(p => p.Profile);
            var userProjects = projects.Where(p => p.Profile.Email == currentUser);

            return userProjects.ToList();
        }

        public static Project FindProject(int? id)
        {
            Project project = db.Projects.Find(id);
            return project;
        }

        public static void CreateProject([Bind(Include = "ID,ProjectName,ProjectDescription,ProfileID")] Project project)
        {
            var userID = HttpContext.Current.User.Identity.GetUserName();
            project.ProfileID = db.Profiles.Where(x => x.Email == userID).Select(x => x.ID).Single();
            db.Projects.Add(project);
            db.SaveChanges();
        }

        public static void EditProject(Project project)
        {
            var currentProject = db.Projects.FirstOrDefault(p => p.ID == project.ID);
            currentProject.ProjectName = project.ProjectName;
            currentProject.ProjectDescription = project.ProjectDescription;
            db.SaveChanges();
        }

        public static void DeleteProjectConfirmation(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
        }

        public static List<Bug> GetBugsForCurrentProject(string id)
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();
            var bugs = db.Bugs.Include(b => b.Project);
            var allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();
            return allBugs;
        }

        public static Bug FindBug(int? id)
        {
            Bug bug = db.Bugs.Find(id);
            return bug;
        }

        public static void CreateBug([Bind(Include = "BugName,Priority,BugDescription,ProjectID")] Bug bug, string id)
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();
            bug.ProjectID = db.Projects.Where(x => x.ProjectName == id && x.Profile.Email == currentUser).Select(x => x.ID).Single();
            db.Bugs.Add(bug);
            db.SaveChanges();
        }

        public static Bug GetCurrentBug(Bug bug)
        {
            Bug currentBug = db.Bugs.FirstOrDefault(b => b.ID == bug.ID);
            return currentBug; 
        }

        public static void EditBug(Bug bug, int id)
        {
            Bug currentBug = db.Bugs.FirstOrDefault(b => b.ID == bug.ID);
            currentBug.BugName = bug.BugName;
            currentBug.BugDescription = bug.BugDescription;
            currentBug.Priority = bug.Priority;
            db.SaveChanges();
        }

        public static void DeleteBugConfirmation(Bug bug)
        {
            db.Bugs.Remove(bug);
            db.SaveChanges();
        }

        public static List<Bug> GetPriorityBugs(string priority, string id)
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();

            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            var bugsWithChosenPriority = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID
                    && b.Priority.ToString() == priority).ToList();

            return bugsWithChosenPriority;
        }

        public static List<Bug> SortBugsByRecent(string id)
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);

            List<Bug> allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();
            allBugs.Reverse();

            return allBugs;
        }

        public static IOrderedEnumerable<Bug> SortBugsByPriority(string id)
        {
            string currentUser = HttpContext.Current.User.Identity.GetUserName();
            int currentProfileID = db.Profiles.Where(p => p.Email == currentUser).Select(p => p.ID).Single();

            var bugs = db.Bugs.Include(b => b.Project);
            List<Bug> allBugs = bugs.Where(b => b.Project.ProjectName == id && b.Project.ProfileID == currentProfileID).ToList();

            var sortOrder = new Dictionary<string, int>
            {
                { "Low", 1 },
                { "Normal", 2 },
                { "High", 3 },
                { "Immediate", 4 }
            };

            int defaultOrder = sortOrder.Max(x => x.Value) + 1;
            IOrderedEnumerable<Bug> sortedByPriority = allBugs.OrderBy(p => sortOrder.TryGetValue(p.Priority.ToString(), out var order) ? order : defaultOrder);

            return sortedByPriority;
        }
    }
}