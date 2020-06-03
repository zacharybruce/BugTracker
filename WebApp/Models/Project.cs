using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Project
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Project name is required"), Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        public string ProjectDescription { get; set; }

        public int ProfileID { get; set; }

        public virtual Profile Profile { get; set; }
    }
}