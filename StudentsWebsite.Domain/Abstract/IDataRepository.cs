using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.Domain.Abstract
{
    public interface IDataRepository
    {
        IEnumerable<Rating> Ratings { get;  }
        IEnumerable<User> Users { get; }
        void SaveUser(User user);
        bool UserNameExists(string username);
        User GetUser(string username);
        void SaveRatings(IEnumerable<Rating> ratings);
        void SaveRatings(IEnumerable<Rating> ratings, string forUserName);
    }
}
