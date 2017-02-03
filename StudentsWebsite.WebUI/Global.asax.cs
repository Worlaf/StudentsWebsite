using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Ninject;

using System.Diagnostics;

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
           
            Domain.Abstract.IDataRepository dataRepository = DependencyResolver.Current.GetService<IKernel>().Get<Domain.Abstract.IDataRepository>();
            if (dataRepository == null)
            {
                return;
            }

            if (Application != null)
            {
                Application["TotalStudents"] = dataRepository.Users.Count(u => u.Role == Domain.Entities.User.Roles.Student);
                Application["TotalLecturers"] = dataRepository.Users.Count(u => u.Role == Domain.Entities.User.Roles.Lecturer);
            }
            if (Context.User != null && dataRepository.GetUser(User.Identity.Name) != null &&
                Context.Session != null && Session["UserData"] == null )
            {
                Domain.Entities.User user = dataRepository.GetUser(User.Identity.Name);
                Session["UserData"] = user;
                Session["FullName"] = user.FirstName + " " + user.LastName;
                Session["LastPage"] = Context.Request.Url;
                switch (user.Role)
                {
                    case Domain.Entities.User.Roles.Student:
                        Session["Group"] = "Студенты"; break;
                    case Domain.Entities.User.Roles.Lecturer:
                        Session["Group"] = "Преподаватели"; break;
                    case Domain.Entities.User.Roles.Dean:
                        Session["Group"] = "Деканат"; break;
                }
            }
        }
       
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Domain.Abstract.IDataRepository dataRepository = DependencyResolver.Current.GetService<IKernel>().Get<Domain.Abstract.IDataRepository>();
            if (Context.User != null && dataRepository.GetUser(User.Identity.Name) != null)
            {
                Domain.Entities.User user = dataRepository.GetUser(User.Identity.Name);
                Debug.Print("AuthenticateRequest User = {0}", User.Identity.Name);               
                Context.User = new System.Security.Principal.GenericPrincipal(User.Identity, new string[] { user.Role.ToString() }); 

                
            }
            else
                Debug.Print("AuthenticateRequest User = null");
          
        }
    }
}
