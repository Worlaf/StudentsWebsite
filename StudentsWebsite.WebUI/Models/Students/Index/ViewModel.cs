using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.WebUI.Models.Students.Index
{
    public class ViewModel
    {
        public IEnumerable<Student> Students { get; set; }
    }
}