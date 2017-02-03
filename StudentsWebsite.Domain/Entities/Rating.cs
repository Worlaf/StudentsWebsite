using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsWebsite.Domain.Entities
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Rating
    {
        public int RatingId { get; set; }
        public string Student_UserName { get; set; }
        public string Lecturer_UserName { get; set; }

        /// <summary>
        /// Оценка по 100бальной системе
        /// </summary>
        public int Rate { get; set; }
    }
}
