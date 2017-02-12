using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.Domain.Abstract
{
    public interface IDataRepositoryOld
    {
        IEnumerable<Rating> Ratings { get;  }
        IEnumerable<DbUser> Users { get; }
        void SaveUser(DbUser user);
        bool UserNameExists(string username);
        DbUser GetUser(string username);
        void SaveRatings(IEnumerable<Rating> ratings);
        void SaveRatings(IEnumerable<Rating> ratings, string forUserName);
    }
}
