using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentsWebsite.Data.Abstract;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.WebUI.Utility;


namespace StudentsWebsite.WebUI.Controllers
{
    public class LecturersController : Controller
    {
        public IDataRepositoryOld DataRepository { get; set; }
        public LecturersController(IDataRepositoryOld dataRep)
        {
            DataRepository = dataRep;
        }
        
        public ActionResult Index()
        {
            var lecturers = DataRepository.Users.Where(u => u.Role == UserRoles.Lecturer).ToArray();
            var models = lecturers.Select(l => new Models.LecturerViewModel
            {
                UserName = l.Email,
                FullName = l.FirstName + l.LastName,
                Subject = l.Subject,
                StudentsCount = DataRepository.Ratings.Count(r => r.Lecturer_UserName == l.Email)
            });
           
            return View(models);
        }
        
        [CustomAuthorize(UserRoles.Dean, UserRoles.Lecturer)]
        public ActionResult Card(string userName = "")
        {
            var viewModel = new Models.LecturerEditViewModel();
            DbUser user = null;
            if (userName != "")
            {
                user = DataRepository.Users.FirstOrDefault(u => u.Email == userName && u.Role == UserRoles.Lecturer);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
                return RedirectToAction("Error");

            viewModel.Lecturer = user;
            viewModel.LecturerUserName = user.Email;

            var ratings = DataRepository.Ratings.Where(r => r.Lecturer_UserName == user.Email).ToArray();
            viewModel.Students = ratings.Select(r =>
            {
                var student = DataRepository.GetUser(r.Student_UserName);
                return new Models.LecturerEditViewModel.StudentSelection
                {
                    StudentUserName = student.Email,
                    StudentFullName = student.FirstName + " " + student.LastName,
                    Rating = r.Rate.Value,
                    Selected = true
                };
            }).ToArray();

            return View(viewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "Dean, Lecturer")]
        [CustomAuthorize(UserRoles.Dean, UserRoles.Lecturer)]
        public ActionResult Card(Models.LecturerEditViewModel viewModel)
        {
            if (!viewModel.LecturerUserName.IsEmpty() && viewModel.Students != null && (HttpContext.User.IsDean() || HttpContext.User.Identity.Name == viewModel.LecturerUserName))
            {
                var students = viewModel.Students;
                var newRatings = students.Where(s => DataRepository.GetUser(s.StudentUserName) != null).Select(s =>
                {
                    var student = DataRepository.GetUser(s.StudentUserName);
                    return new Rating
                    {
                        Student_UserName = student.Email,
                        Lecturer_UserName = viewModel.LecturerUserName,
                        Rate = s.Rating
                    };
                });              

                DataRepository.SaveRatings(newRatings, viewModel.LecturerUserName);

                return RedirectToAction("Card", new { userName = viewModel.LecturerUserName });             
            }
            else
            {
                // Что-то не так со значениями данных
                return Card(viewModel.LecturerUserName ?? viewModel.Lecturer.Email);
            }
        }

        [Authorize(Roles = "Dean")]
        public ActionResult Edit(string userName = "")
        {
            var viewModel = new Models.LecturerEditViewModel();
            DbUser user = null;
            if (userName != "")
            {
                user = DataRepository.Users.FirstOrDefault(u => u.Email == userName && u.Role == UserRoles.Lecturer);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
            {
                //Random generation for testing purposes
                string firstName, lastName;
                Helpers.RandomDataGenerator.RandomName(out firstName, out lastName);
                user = new DbUser
                {
                    
                    Email = "",
                    FirstName = firstName,
                    LastName = lastName,
                    Role = UserRoles.Lecturer,
                };
                ViewBag.ActionString = "Добавить";
            }
            viewModel.Lecturer = user;

            var ratings = DataRepository.Ratings.Where(r => r.Lecturer_UserName == user.Email).ToArray();
            var students = DataRepository.Users.Where(u => u.Role == UserRoles.Student).ToArray();
            Models.LecturerEditViewModel.StudentSelection[] studentSelections = new Models.LecturerEditViewModel.StudentSelection[students.Length];
            var studentsSelection = students.Select(s =>
            {
                var rating = ratings.FirstOrDefault(r => r.Student_UserName == s.Email);
                return new Models.LecturerEditViewModel.StudentSelection{
                    StudentUserName = s.Email,
                    StudentFullName = s.FirstName + " " + s.LastName,
                    Rating = rating != null ? rating.Rate.Value : -1,
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
                user.Role = UserRoles.Lecturer;
                DataRepository.SaveUser(user);

                var students = viewModel.Students.Where(lect => lect.Selected);
                var newRatings = students.Where(s => DataRepository.GetUser(s.StudentUserName) != null)
                    .Select(s => new Rating
                    {
                        Student_UserName = s.StudentUserName,
                        Lecturer_UserName = user.Email,
                        Rate = s.Rating
                    });                
                DataRepository.SaveRatings(newRatings, user.Email);

                return RedirectToAction("Card", new { userName = viewModel.Lecturer.Email });
            }
            else
            {
                // Что-то не так со значениями данных
                return Edit(viewModel.LecturerUserName ?? viewModel.Lecturer.Email);
            }            
        }
	}
}