using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.WebUI.Utility
{    
    public static class UserExtensions
    {
        public const string Dean = "Dean";
        public const string Lecturer = "Lecturer";
        public const string Student = "Student";

        public static bool IsDean(this System.Security.Principal.IPrincipal user)
        {
            return user.IsInRole(Dean);
        }

        public static bool IsLecturer(this System.Security.Principal.IPrincipal user)
        {
            return user.IsInRole(Lecturer);
        }

        public static bool IsStudent(this System.Security.Principal.IPrincipal user)
        {
            return user.IsInRole(Student);
        }
    }
}