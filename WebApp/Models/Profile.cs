using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Bug> Bugs { get; set; }
        public virtual ICollection<Project> Projects { get; set; }


    }
}