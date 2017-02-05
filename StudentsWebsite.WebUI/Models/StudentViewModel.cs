using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.WebUI.Models
{
    public class StudentViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int SubjectCount { get; set; }
        public int AverageRating { get; set; }
    }
}