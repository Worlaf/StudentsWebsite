using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.Domain.Entities
{
    public class TEntity
    {
        [Key]
        public uint Id { get; set; }
    }
}
