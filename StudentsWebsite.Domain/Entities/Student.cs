using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.Data.Entities
{
    public class Student:BdEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public DbUser User { get; set; }
        public double? AverageRating { get; set; }
        public int SubjectsCount { get; set; }

        public IEnumerable<Lecturer> Lecturers { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
    }
}
