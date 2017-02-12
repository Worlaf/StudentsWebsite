using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Ninject;

using System.Diagnostics;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        public void Application_AcquireRequestState(object sender, EventArgs e)
        {
           
            Domain.Abstract.IDataRepositoryOld dataRepository = DependencyResolver.Current.GetService<IKernel>().Get<Domain.Abstract.IDataRepositoryOld>();
            if (dataRepository == null)
            {
                return;
            }

            if (Application != null)
            {
                Application["TotalStudents"] = dataRepository.Users.Count(u => u.Role == UserRoles.Student);
                Application["TotalLecturers"] = dataRepository.Users.Count(u => u.Role == UserRoles.Lecturer);
            }
            if (Context.User != null && dataRepository.GetUser(User.Identity.Name) != null &&
                Context.Session != null && Session["UserData"] == null )
            {
                Domain.Entities.DbUser user = dataRepository.GetUser(User.Identity.Name);
                Session["UserData"] = user;
                Session["FullName"] = user.FirstName + " " + user.LastName;
                Session["LastPage"] = Context.Request.Url;
                switch (user.Role)
                {
                    case UserRoles.Student:
                        Session["Group"] = "Студенты"; break;
                    case UserRoles.Lecturer:
                        Session["Group"] = "Преподаватели"; break;
                    case UserRoles.Dean:
                        Session["Group"] = "Деканат"; break;
                }
            }
        }
       
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Domain.Abstract.IDataRepositoryOld dataRepository = DependencyResolver.Current.GetService<IKernel>().Get<Domain.Abstract.IDataRepositoryOld>();
            if (Context.User != null && dataRepository.GetUser(User.Identity.Name) != null)
            {
                Domain.Entities.DbUser user = dataRepository.GetUser(User.Identity.Name);
                Debug.Print("AuthenticateRequest User = {0}", User.Identity.Name);               
                Context.User = new System.Security.Principal.GenericPrincipal(User.Identity, new string[] { user.Role.ToString() }); 

                
            }
            else
                Debug.Print("AuthenticateRequest User = null");
          
        }
    }
}
