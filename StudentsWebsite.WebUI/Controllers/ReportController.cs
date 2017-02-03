using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.WebUI.Controllers
{
    public class ReportController : Controller
    {
        IDataRepository dataRepository;
        public ReportController(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return RedirectToAction("TopStudents");
        }

        [Authorize(Roles = "Dean")]
        public ActionResult TopStudents()
        {
            User[] students = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Student).ToArray();
            Models.StudentViewModel[] models = new Models.StudentViewModel[students.Length];

            int subjectsCount;
            double averageRating;

            double totalAverage = 0;

            IEnumerable<Rating> ratings;

            for (int i = 0; i < models.Length; i++)
            {
                ratings = dataRepository.Ratings.Where(r => r.Student_UserName == students[i].UserName);
                
                subjectsCount = ratings.Count(r => r.Rate >= 0);
                if (subjectsCount > 0)
                    averageRating = ratings.Sum(r => r.Rate >= 0 ? (double)r.Rate : 0) / subjectsCount;
                else
                    averageRating = 0;
                totalAverage += averageRating;

                models[i] = new Models.StudentViewModel()
                {
                    AverageRating = (int)averageRating,
                    SubjectCount = subjectsCount,
                    UserName = students[i].UserName,
                    FullName = students[i].FirstName + " " + students[i].LastName
                };
            }

            totalAverage /= students.Length;

            models = models.Where(m => m.AverageRating > totalAverage).ToArray();

            ViewBag.TotalAverage = (int)totalAverage;

            return View(models);
        }
        [Authorize(Roles = "Dean")]
        public ActionResult PopularLecturers()
        {
            User[] lecturers = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Lecturer).ToArray();
            int allStudentsCount = dataRepository.Users.Count(u => u.Role == Domain.Entities.User.Roles.Student);
            int ratingsCount;

            List<Models.LecturerViewModel> models = new List<Models.LecturerViewModel>();
            Models.LecturerViewModel model;

            for (int i = 0; i < lecturers.Length; i++)
            {
                ratingsCount = dataRepository.Ratings.Count(r => r.Lecturer_UserName == lecturers[i].UserName);

                if (ratingsCount == allStudentsCount)
                {
                    model = new Models.LecturerViewModel()
                    {
                        FullName = lecturers[i].FirstName + " " + lecturers[i].LastName,
                        UserName = lecturers[i].UserName,
                        StudentsCount = ratingsCount,
                        Subject = lecturers[i].Subject
                    };

                    models.Add(model);
                }
            }

            return View(models.ToArray());

        }
        [Authorize(Roles = "Dean")]
        public ActionResult UnpopularLecturers()
        {
            User[] lecturers = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Lecturer).ToArray();           
            int ratingsCount;
            int minimum = Int32.MaxValue;
            Models.LecturerViewModel[] models = new Models.LecturerViewModel[lecturers.Length];
            Models.LecturerViewModel model;

            for (int i = 0; i < lecturers.Length; i++)
            {
                ratingsCount = dataRepository.Ratings.Count(r => r.Lecturer_UserName == lecturers[i].UserName);
                if (ratingsCount < minimum)
                    minimum = ratingsCount;
              
                    model = new Models.LecturerViewModel()
                    {
                        FullName = lecturers[i].FirstName + " " + lecturers[i].LastName,
                        UserName = lecturers[i].UserName,
                        StudentsCount = ratingsCount,
                        Subject = lecturers[i].Subject
                    };

                    models[i] = model;
                
            }

            minimum = minimum + 1;

            models = models.Where(m => m.StudentsCount <= minimum).ToArray();

            return View(models);
        }
	}
}