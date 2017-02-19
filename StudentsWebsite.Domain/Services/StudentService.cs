using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Repositories;

namespace StudentsWebsite.Data.Services
{
    public interface IStudentService : IDbEntityService<Student>
    {
        
    }
    public class StudentService:IStudentService
    {
        public IDbUserService DbUserService { get; set; }
        public IDbUserRepository DbUserRepository { get; set; }
        [Inject]
        public IStudentRepository StudentRepository { get; set; }
        public void Save(Student entity)
        {
            StudentRepository.Save(entity);
        }
    }
}
