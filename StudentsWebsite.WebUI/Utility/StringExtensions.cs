using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.WebUI.Utility
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return source == null || source.Trim() == "";
        }
    }
}