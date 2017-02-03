using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.WebUI.Infrastructure.Concrete
{
    public class DBAuthProvider : Infrastructure.Abstract.IAuthProvider
    {
        private Domain.Abstract.IDataRepository dataRepository;

        public DBAuthProvider(Domain.Abstract.IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public bool ValidateUser(string username, string password)
        {
            User user = dataRepository.Users.FirstOrDefault (u => u.UserName == username && u.Password == password);
            return user != null;
        }
        public bool Login(string username, string password)
        {
            return Login(username, password, true);
        }
        public bool Login(string username, string password, bool persistant)
        {
            if (ValidateUser(username, password))
            {                
                FormsAuthentication.SetAuthCookie(username, false);
                return true;
            }
            return false;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}