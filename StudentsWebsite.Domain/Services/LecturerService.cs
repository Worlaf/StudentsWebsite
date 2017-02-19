using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Repositories;

namespace StudentsWebsite.Data.Services
{
    public interface ILecturerService : IDbEntityService<Lecturer>
    {
        
    }
    public class LecturerService : ILecturerService
    {
        [Inject]
        public ILecturerRepository LecturerRepository { get; set; }
        public void Save(Lecturer entity)
        {
            LecturerRepository.Save(entity);
        }
    }
}
