using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentsWebsite.WebUI.Models
{
    public class LecturerEditViewModel
    {
        public class StudentSelection
        {
            public string StudentUserName { get; set; }
            public string StudentFullName { get; set; }
            public int Rating { get; set; }
            public bool Selected { get; set; }
        }
        
        public string LecturerUserName { get; set; }
        public DbUser Lecturer { get; set; }
        public StudentSelection[] Students { get; set; }
    }
}