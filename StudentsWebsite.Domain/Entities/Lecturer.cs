using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.Domain.Entities
{
    public class Lecturer : TEntity
    {
        public uint UserId { get; set; }
        public DbUser User { get; set; }

        public string Subject { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}