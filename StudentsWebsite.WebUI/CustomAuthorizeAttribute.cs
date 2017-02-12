using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.WebUI.Utility;

namespace StudentsWebsite.WebUI
{
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IEnumerable<UserRoles> Roles { get; set; }
        public CustomAuthorizeAttribute()
        {

        }
        public CustomAuthorizeAttribute(params UserRoles[] roles)
        {
            Roles = roles;
        }
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            return httpContext.User.CheckRoles(Roles.ToArray());
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
                HandleUnauthorizedRequest(filterContext);

        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = (ActionResult)new HttpUnauthorizedResult();
        }
    }
}