using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.WebUI.Models
{
    public class LecturerViewModel
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public int StudentsCount { get; set; }

        public string Subject { get; set; }
    }
}