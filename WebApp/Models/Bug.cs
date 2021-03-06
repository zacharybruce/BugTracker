﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Bug
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Bug name required"), Display(Name = "Bug Name")]
        public string BugName { get; set; }

        [Required(ErrorMessage = "Please set priority for bug")]
        public Priority Priority { get; set; }

        [Display(Name = "Description")]
        public string BugDescription { get; set; }

        public string Status { get; set; }

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

    public enum Status
    {
        Active,
        Solved
    }
}