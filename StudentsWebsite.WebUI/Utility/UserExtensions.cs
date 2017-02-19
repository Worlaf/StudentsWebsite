using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.WebUI.Utility
{    
    public static class UserExtensions
    {
        public const string Dean = "Dean";
        public const string Lecturer = "Lecturer";
        public const string Student = "Student";

        public static bool IsDean(this System.Security.Principal.IPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(Dean);
        }

        public static bool IsLecturer(this System.Security.Principal.IPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(Lecturer);
        }

        public static bool IsStudent(this System.Security.Principal.IPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(Student);
        }

        public static bool CheckRoles(this System.Security.Principal.IPrincipal user, params UserRoles[] roles)
        {
            foreach (var role in roles)
            {
                if (role == UserRoles.Dean && user.IsDean())
                    return true;
                if (role == UserRoles.Lecturer && user.IsLecturer())
                    return true;
                if (role == UserRoles.Student && user.IsStudent())
                    return true;
            }

            return false;
        }
    }
}