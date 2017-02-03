using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsWebsite.Domain.Entities
{
    public class User
    {
        /// <summary>
        /// Возможные роли пользователя
        /// </summary>
        public enum Roles { 
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

        [StringLength(64)]
        [Index(IsUnique=true)]
        [Key]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Roles Role { get; set; }

        //Можно было бы предметы выделить в отдельную таблицу
        //что позволило бы в будущем добавить возможность вести
        //несколько предметов одному преподавателю
        //Однако в задании указано что преподаватель ведет один предмет
        //и студенты посещают именно преподавателя
        public string Subject { get; set; }
        
    }
}
