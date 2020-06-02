using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Bug
    {
        public int ID { get; set; }
        public string BugName { get; set; }
        public Priority Priority { get; set; }
        public string BugDescription { get; set; }
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }
    }

    public enum Priority
    {
        Immediate,
        High,
        Normal,
        Low
    }
}