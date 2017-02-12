using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsWebsite.Domain
{
    public static class DbUserExtensions
    {
        public static string FullName(Entities.DbUser self)
        {
            return $"{self.FirstName} {self.LastName}";
        }
    }
}
