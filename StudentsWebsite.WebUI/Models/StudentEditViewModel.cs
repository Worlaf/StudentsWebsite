using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentsWebsite.WebUI.Models
{
    public class StudentEditViewModel
    {
        public class LecturerSelection
        {
            public string LecturerUserName { get; set; }
            public string LecturerFullName { get; set; }
            public string LecturerSubject { get; set; }
            public int Rating { get; set; }
            public bool Selected { get; set; }
        }

        public DbUser Student { get; set; }
        public string StudentUserName { get; set; }
        public LecturerSelection[] Lecturers { get; set; }

    }
}