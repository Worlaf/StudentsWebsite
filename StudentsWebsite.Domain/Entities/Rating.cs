using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsWebsite.Data.Entities
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Rating : BdEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        //Заменить _UserName на Id во всех использованиях
        public string Student_UserName { get; set; }
        public string Lecturer_UserName { get; set; }
        /// <summary>
        /// Оценка по 100бальной системе
        /// </summary>
        public int? Rate { get; set; }
    }
}
