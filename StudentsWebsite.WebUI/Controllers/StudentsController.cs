using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Ninject;
using StudentsWebsite.Data.Abstract;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Repositories;
using StudentsWebsite.WebUI.Utility;

namespace StudentsWebsite.WebUI.Controllers
{
    public class StudentsController : Controller
    {
        IDataRepositoryOld dataRepository;

        [Inject]
        public IDbUserRepository DbUserRepository { get; set; }
        [Inject]
        public IStudentRepository StudentRepository { get; set; }

        public StudentsController(IDataRepositoryOld dataRepository)
        {
            this.dataRepository = dataRepository;
        }
       
        public ActionResult Index()
        {
            /*var students = dataRepository.Users.Where(u => u.Role == UserRoles.Student).ToArray();
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
            });*/

            var students = StudentRepository.GetAll().ToArray();

            return View(new Models.Students.Index.ViewModel
            {
                Students = students
            });
        }

        [Authorize(Roles = ("Lecturer, Dean, Student"))]
        public ActionResult Card(string userName = "")
        {
            Models.StudentEditViewModel editViewModel = new Models.StudentEditViewModel();
         
            DbUser user = null;
            if (userName != "")
                user = dataRepository.Users.FirstOrDefault(u => u.Email == userName && u.Role == UserRoles.Student);

            if (user == null)
                return RedirectToAction("Error");

            editViewModel.Student = user;
            editViewModel.StudentUserName = user.Email;
           
            var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.Email).ToArray();
            var lecturerSelections = ratings.Select(r =>
            {
                var lecturer = dataRepository.GetUser(r.Lecturer_UserName);
                return new Models.StudentEditViewModel.LecturerSelection
                {
                    LecturerUserName = r.Lecturer_UserName,
                    LecturerFullName = lecturer.FirstName + " " + lecturer.LastName,
                    LecturerSubject = lecturer.Subject,
                    Rating = r.Rate.Value,
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
                return Card(viewModel.StudentUserName ?? viewModel.Student.Email);
        }

        [Authorize(Roles = ("Lecturer, Dean"))]
        public ActionResult Edit(string userName = "")
        {
            var editViewModel = new Models.StudentEditViewModel();

            DbUser user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.Email == userName && u.Role == UserRoles.Student);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
            {                
                string firstName, lastName;
                Helpers.RandomDataGenerator.RandomName(out firstName, out lastName);
                user = new DbUser
                {

                    Email = "",
                    FirstName = firstName,
                    LastName = lastName,
                    Role = UserRoles.Student,
                };

                ViewBag.ActionString = "Добавить";
            }
            editViewModel.Student = user;

            var ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.Email);
            var lecturers = dataRepository.Users.Where(u => u.Role == UserRoles.Lecturer);
            editViewModel.Lecturers = lecturers.Select(l =>
            {
                var rating = ratings.FirstOrDefault(r => r.Lecturer_UserName == l.Email);
                return new Models.StudentEditViewModel.LecturerSelection
                {
                    LecturerUserName = l.Email,
                    LecturerFullName = l.FirstName + " " + l.LastName,
                    LecturerSubject = l.Subject,
                    Rating = rating != null ? rating.Rate.Value : -1,
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
                        Student_UserName = user.Email,
                        Lecturer_UserName = l.LecturerUserName,
                        Rate = l.Rating
                    });
                dataRepository.SaveRatings(newRatings, user.Email);

                return RedirectToAction("Card", new { userName = editViewModel.Student.Email });
            }
            else
                return Edit(editViewModel.StudentUserName);

        }
	}
}