using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsWebsite.Domain.Entities
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Rating : TEntity
    {
        public uint StudentId { get; set; }
        public DbUser Student { get; set; }
        public uint LecturerId { get; set; }
        public DbUser Lecturer { get; set; }

        //Заменить _UserName на Id во всех использованиях
        public string Student_UserName { get; set; }
        public string Lecturer_UserName { get; set; }
        /// <summary>
        /// Оценка по 100бальной системе
        /// </summary>
        public int Rate { get; set; }
    }
}
