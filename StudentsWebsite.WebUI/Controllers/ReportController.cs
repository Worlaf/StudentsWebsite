using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Data.Abstract;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.WebUI.Controllers
{
    public class ReportController : Controller
    {
        IDataRepositoryOld dataRepository;
        public ReportController(IDataRepositoryOld dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("TopStudents");
        }

        [Authorize(Roles = "Dean")]
        public ActionResult TopStudents()
        {
            var students = dataRepository.Users.Where(u => u.Role == UserRoles.Student).ToArray();           
            double totalAverage = 0;
            var models = students
                .Select(s =>
                {
                    var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == s.Email);
                    var subjectsCount = ratings.Count(r => r.Rate >= 0);
                    var average = subjectsCount > 0 ? ratings.Sum(r => r.Rate >= 0 ? (double)r.Rate : 0) / subjectsCount : 0;
                    totalAverage += average / students.Length;
                    return new Models.StudentViewModel
                    {
                        AverageRating = (int)average,
                        SubjectCount = subjectsCount,
                        UserName = s.Email,
                        FullName = s.FirstName + " " + s.LastName
                    };
                })
                .Where(m => m.AverageRating > totalAverage)
                .ToArray();
            
            ViewBag.TotalAverage = (int)totalAverage;

            return View(models);
        }

        [Authorize(Roles = "Dean")]
        public ActionResult PopularLecturers()
        {
            var lecturers = dataRepository.Users.Where(u => u.Role == UserRoles.Lecturer).ToArray();
            var allStudentsCount = dataRepository.Users.Count(u => u.Role == UserRoles.Student);
            var models = lecturers
                .Where(l => dataRepository.Ratings.Count(r => r.Lecturer_UserName == l.Email) == allStudentsCount)
                .Select(l => new Models.LecturerViewModel
                {
                    FullName = l.FirstName + " " + l.LastName,
                    UserName = l.Email,
                    StudentsCount = dataRepository.Ratings.Count(r => r.Lecturer_UserName == l.Email),
                    Subject = l.Subject
                })
                .ToArray();

            return View(models);
        }
        [Authorize(Roles = "Dean")]
        public ActionResult UnpopularLecturers()
        {
            var lecturers = dataRepository.Users.Where(u => u.Role == UserRoles.Lecturer).ToArray();   
            var minimum = lecturers.Min(l => (decimal?)dataRepository.Ratings.Count(r => r.Lecturer_UserName == l.Email)) + 1;
            var models = lecturers
                .Select(l => new Models.LecturerViewModel
                {
                    FullName = l.FirstName + " " + l.LastName,
                    UserName = l.Email,
                    StudentsCount = dataRepository.Ratings.Count(r => r.Lecturer_UserName == l.Email),
                    Subject = l.Subject
                })
                .Where(m => m.StudentsCount <= minimum);            

            return View(models);
        }
	}
}