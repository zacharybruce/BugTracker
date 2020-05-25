using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.DataAccess
{
    public class BugTrackerContext : DbContext
    {
        public BugTrackerContext() : base("BugTrackerContext")
        {
            Database.SetInitializer<BugTrackerContext>(new DropCreateDatabaseIfModelChanges<BugTrackerContext>());
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}