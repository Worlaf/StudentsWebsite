using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.WebUI.Infrastructure.Abstract;
using StudentsWebsite.WebUI.Infrastructure.Concrete;
using StudentsWebsite.WebUI.Models;

namespace StudentsWebsite.WebUI.Controllers
{
    public class AccountController : Controller
    {
        protected IAuthProvider authProvider;

        public AccountController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Login(login.UserName, login.Password))
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
                return View();
        }

        public ActionResult Logout(string returnUrl = "")
        {
            authProvider.Logout();
            ControllerContext.HttpContext.User = null;
            HttpContext.Session["UserData"] = null;
            HttpContext.Session["FullName"] = null;
            HttpContext.Session["Group"] = null;
            return Redirect(returnUrl);
        }

       
	}
}