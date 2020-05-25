using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Bug
    {
        public int ID { get; set; }
        public string BugName { get; set; }
        public string Priority { get; set; }
        public string BugDescription { get; set; }
        public int ProfileID { get; set; }

        public virtual Profile Profile { get; set; }
    }
}