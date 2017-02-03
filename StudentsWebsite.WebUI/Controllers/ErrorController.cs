using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsWebsite.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int errCode)
        {
            return View();
        }
	}
}