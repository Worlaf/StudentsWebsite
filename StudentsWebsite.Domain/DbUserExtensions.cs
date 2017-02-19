using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data
{
    public static class DbUserExtensions
    {
        public static string FullName(this DbUser self)
        {
            return $"{self.FirstName} {self.LastName}";
        }
    }
}
