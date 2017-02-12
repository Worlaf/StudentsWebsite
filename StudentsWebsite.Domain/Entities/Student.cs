using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.Domain.Entities
{
    public class Student:TEntity
    {
        public uint UserId { get; set; }
        public DbUser User { get; set; }

        public IEnumerable<Lecturer> Lecturers { get; set; }
    }
}
