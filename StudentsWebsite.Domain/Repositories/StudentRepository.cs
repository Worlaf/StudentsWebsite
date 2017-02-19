using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Ninject;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Repositories
{
    public interface IStudentRepository : IDataRepository<Student>
    {
        Student ByEmail(string email);
    }
    public class StudentRepository : DataRepositoryBase<Entities.Student>, IStudentRepository
    {
        [Inject]
        public IRatingRepository RatingRepository { get; set; }
        [Inject]
        public IDbUserRepository DbUserRepository { get; set; }

        public override IQueryable<Student> GetAll()
        {
            return base.GetAll().Include(s => s.User);
        }

        public Student ByEmail(string email)
        {
            return Single(s => s.User.Email == email);
        }
    }
}
