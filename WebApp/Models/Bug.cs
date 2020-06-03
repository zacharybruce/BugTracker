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

        [Required, Display(Name = "Bug Name")]
        public string BugName { get; set; }
        public Priority Priority { get; set; }
        [Display(Name = "Description")]
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