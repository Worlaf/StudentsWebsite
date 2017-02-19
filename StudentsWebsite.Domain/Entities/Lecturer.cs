using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentsWebsite.Data.Entities
{
    public class Lecturer : BdEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public DbUser User { get; set; }
        public string Subject { get; set; }
        public int StudentsCount { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}