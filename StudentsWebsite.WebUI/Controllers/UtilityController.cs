using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.WebUI.Controllers
{
    public class UtilityController : Controller
    {
        IDataRepository dataRepository;
        public UtilityController(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateTable()
        {
            Helpers.TestDataTableGenerator.FillTable(dataRepository);

            return View();
        }
    }
}