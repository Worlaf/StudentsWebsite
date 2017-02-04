using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.WebUI.Utility;


namespace StudentsWebsite.WebUI.Controllers
{
    public class LecturersController : Controller
    {
        IDataRepository dataRepository;
        public LecturersController(IDataRepository dataRep)
        {
            dataRepository = dataRep;
        }
        
        public ActionResult Index()
        {
            var lecturers = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Lecturer).ToArray();
            var models = lecturers.Select(l => new Models.LecturerViewModel
            {
                UserName = l.UserName,
                FullName = l.FirstName + l.LastName,
                Subject = l.Subject,
                StudentsCount = dataRepository.Ratings.Count(r => r.Lecturer_UserName == l.UserName)
            });
           
            return View(models);
        }

        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Card(string userName = "")
        {
            var viewModel = new Models.LecturerEditViewModel();
            User user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == Domain.Entities.User.Roles.Lecturer);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
                return RedirectToAction("Error");

            viewModel.Lecturer = user;
            viewModel.LecturerUserName = user.UserName;

            var ratings = dataRepository.Ratings.Where(r => r.Lecturer_UserName == user.UserName).ToArray();
            viewModel.Students = ratings.Select(r =>
            {
                var student = dataRepository.GetUser(r.Student_UserName);
                return new Models.LecturerEditViewModel.StudentSelection
                {
                    StudentUserName = student.UserName,
                    StudentFullName = student.FirstName + " " + student.LastName,
                    Rating = r.Rate,
                    Selected = true
                };
            }).ToArray();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Card(Models.LecturerEditViewModel viewModel)
        {
            if (!viewModel.LecturerUserName.IsNullOrEmpty() && viewModel.Students != null && (HttpContext.User.IsDean() || HttpContext.User.Identity.Name == viewModel.LecturerUserName))
            {
                var students = viewModel.Students;
                var newRatings = students.Where(s => dataRepository.GetUser(s.StudentUserName) != null).Select(s =>
                {
                    var student = dataRepository.GetUser(s.StudentUserName);
                    return new Rating
                    {
                        Student_UserName = student.UserName,
                        Lecturer_UserName = viewModel.LecturerUserName,
                        Rate = s.Rating
                    };
                });              

                dataRepository.SaveRatings(newRatings, viewModel.LecturerUserName);

                return RedirectToAction("Card", new { userName = viewModel.LecturerUserName });             
            }
            else
            {
                // Что-то не так со значениями данных
                return Card(viewModel.LecturerUserName ?? viewModel.Lecturer.UserName);
            }
        }

        [Authorize(Roles = "Dean")]
        public ActionResult Edit(string userName = "")
        {
            var viewModel = new Models.LecturerEditViewModel();
            User user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == Domain.Entities.User.Roles.Lecturer);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
            {
                //Random generation for testing purposes
                string firstName, lastName;
                Helpers.RandomDataGenerator.RandomName(out firstName, out lastName);
                user = new User
                {
                    
                    UserName = "",
                    FirstName = firstName,
                    LastName = lastName,
                    Role = Domain.Entities.User.Roles.Lecturer,
                };
                ViewBag.ActionString = "Добавить";
            }
            viewModel.Lecturer = user;

            var ratings = dataRepository.Ratings.Where(r => r.Lecturer_UserName == user.UserName).ToArray();
            var students = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Student).ToArray();
            Models.LecturerEditViewModel.StudentSelection[] studentSelections = new Models.LecturerEditViewModel.StudentSelection[students.Length];
            var studentsSelection = students.Select(s =>
            {
                var rating = ratings.FirstOrDefault(r => r.Student_UserName == students[i].UserName)
                var selection = new Models.LecturerEditViewModel.StudentSelection{
                    StudentUserName = s.UserName,
                    StudentFullName = s.FirstName + " " + s.LastName,
                    Rating = rating != null ? rating.Rate : -1,
                    Selected = rating != null
                };
            });            
            viewModel.Students = studentSelections;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles="Dean")]
        public ActionResult Edit(Models.LecturerEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = viewModel.Lecturer;
                user.Role = Domain.Entities.User.Roles.Lecturer;
                dataRepository.SaveUser(user);

                var students = viewModel.Students.Where(lect => lect.Selected);
                var newRatings = students.Where(s => dataRepository.GetUser(s.StudentUserName) != null)
                    .Select(s => new Rating
                    {
                        Student_UserName = s.StudentUserName,
                        Lecturer_UserName = user.UserName,
                        Rate = s.Rating
                    });                
                dataRepository.SaveRatings(newRatings, user.UserName);

                return RedirectToAction("Card", new { userName = viewModel.Lecturer.UserName });
            }
            else
            {
                // Что-то не так со значениями данных
                return Edit(viewModel.LecturerUserName ?? viewModel.Lecturer.UserName);
            }            
        }
	}
}