using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.WebUI.Utility;

namespace StudentsWebsite.WebUI.Controllers
{
    public class StudentsController : Controller
    {
        IDataRepositoryOld dataRepository;

        public StudentsController(IDataRepositoryOld dataRepository)
        {
            this.dataRepository = dataRepository;
        }
       
        public ActionResult Index()
        {
            var students = dataRepository.Users.Where(u => u.Role == UserRoles.Student).ToArray();
            var models = students.Select(s => {
                var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == s.UserName);
                var subjectsCount = ratings.Count(r => r.Rate >= 0);
                return new Models.StudentViewModel
                {
                    FullName = s.FirstName + " " + s.LastName,
                    UserName = s.UserName,
                    SubjectCount = subjectsCount,
                    AverageRating = subjectsCount > 0 ? (int)(ratings.Sum(r => r.Rate >= 0 ? (double)r.Rate : 0) / subjectsCount) : 0
                };
            });

            return View(models);
        }

        [Authorize(Roles = ("Lecturer, Dean, Student"))]
        public ActionResult Card(string userName = "")
        {
            Models.StudentEditViewModel editViewModel = new Models.StudentEditViewModel();
         
            DbUser user = null;
            if (userName != "")
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == UserRoles.Student);

            if (user == null)
                return RedirectToAction("Error");

            editViewModel.Student = user;
            editViewModel.StudentUserName = user.UserName;
           
            var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.UserName).ToArray();
            var lecturerSelections = ratings.Select(r =>
            {
                var lecturer = dataRepository.GetUser(r.Lecturer_UserName);
                return new Models.StudentEditViewModel.LecturerSelection
                {
                    LecturerUserName = r.Lecturer_UserName,
                    LecturerFullName = lecturer.FirstName + " " + lecturer.LastName,
                    LecturerSubject = lecturer.Subject,
                    Rating = r.Rate,
                    Selected = true
                };
            }).ToArray();
            editViewModel.Lecturers = lecturerSelections;

            return View(editViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Card(Models.StudentEditViewModel viewModel)
        {
            if (viewModel.StudentUserName.IsEmpty() && viewModel.Lecturers != null && (HttpContext.User.IsDean() || HttpContext.User.IsLecturer()))
            {
                var lecturers = viewModel.Lecturers;
                var newRatings = lecturers
                    .Where(l => dataRepository.GetUser(l.LecturerUserName) != null)
                    .Select(l => new Rating
                    {
                        Lecturer_UserName = l.LecturerUserName,
                        Student_UserName = viewModel.StudentUserName,
                        Rate = l.Rating
                    });

                dataRepository.SaveRatings(newRatings, viewModel.StudentUserName);

                return RedirectToAction("Card", new { userName = viewModel.StudentUserName });
            }
            else
                return Card(viewModel.StudentUserName ?? viewModel.Student.UserName);
        }

        [Authorize(Roles = ("Lecturer, Dean"))]
        public ActionResult Edit(string userName = "")
        {
            var editViewModel = new Models.StudentEditViewModel();

            DbUser user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == UserRoles.Student);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
            {                
                string firstName, lastName;
                Helpers.RandomDataGenerator.RandomName(out firstName, out lastName);
                user = new DbUser
                {

                    UserName = "",
                    FirstName = firstName,
                    LastName = lastName,
                    Role = UserRoles.Student,
                };

                ViewBag.ActionString = "Добавить";
            }
            editViewModel.Student = user;

            var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.UserName);
            var lecturers = dataRepository.Users.Where(u => u.Role == UserRoles.Lecturer);
            editViewModel.Lecturers = lecturers.Select(l =>
            {
                var rating = ratings.FirstOrDefault(r => r.Lecturer_UserName == l.UserName);
                return new Models.StudentEditViewModel.LecturerSelection
                {
                    LecturerUserName = l.UserName,
                    LecturerFullName = l.FirstName + " " + l.LastName,
                    LecturerSubject = l.Subject,
                    Rating = rating != null ? rating.Rate : -1,
                    Selected = rating != null
                };
            }).ToArray();

            return View(editViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Edit(Models.StudentEditViewModel editViewModel)
        {            
            if (ModelState.IsValid)
            {
                var user = editViewModel.Student;
                user.Role = UserRoles.Student;
                dataRepository.SaveUser(user);

                //Достаем список преподавателей-оценок (Ratings) из списка выбранных преподавателей 
                var lecturers = editViewModel.Lecturers.Where(lect => lect.Selected).ToArray();
                var newRatings = lecturers
                    .Where(l => dataRepository.GetUser(l.LecturerUserName) != null)
                    .Select(l => new Rating
                    {
                        Student_UserName = user.UserName,
                        Lecturer_UserName = l.LecturerUserName,
                        Rate = l.Rating
                    });
                dataRepository.SaveRatings(newRatings, user.UserName);

                return RedirectToAction("Card", new { userName = editViewModel.Student.UserName });
            }
            else
                return Edit(editViewModel.StudentUserName);

        }
	}
}