using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool Login(string username, string password);
        bool Login(string username, string password, bool persistant);

        void Logout();

        bool ValidateUser(string username, string password);
    }
}
