using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Repositories;

namespace StudentsWebsite.Data.Services
{
    public interface IDbUserService : IDbEntityService<DbUser>
    {

    }
    public class DbUserService : IDbUserService
    {
        public IDbUserRepository DbUserRepository { get; set; }
        public void Save(DbUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            DbUserRepository.Save(user);
        }
    }
}
