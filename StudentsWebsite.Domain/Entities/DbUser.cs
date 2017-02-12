using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsWebsite.Domain.Entities
{
    public class DbUser : TEntity
    {
        [StringLength(64)]
        [Index(IsUnique=true)]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public UserRoles Role { get; set; }
        public string Subject { get; set; }
    }

    /// <summary>
    /// Возможные роли пользователя
    /// </summary>
    public enum UserRoles { 
        /// <summary>
        /// Студент
        /// </summary>
        Student, 
        /// <summary>
        /// Преподаватель
        /// </summary>
        Lecturer,
        /// <summary>
        /// Декан
        /// </summary>
        Dean 
    }
}
