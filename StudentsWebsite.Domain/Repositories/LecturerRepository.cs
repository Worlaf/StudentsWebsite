using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Repositories
{
    public interface ILecturerRepository : IDataRepository<Lecturer>
    {
        Lecturer ByEmail(string email);
    }
    public class LecturerRepository: DataRepositoryBase<Entities.Lecturer>, ILecturerRepository
    {
        public override IQueryable<Lecturer> GetAll()
        {
            return base.GetAll().Include(l => l.User);
        }

        public Lecturer ByEmail(string email)
        {
            return Single(l => l.User.Email == email);
        }
    }
}
