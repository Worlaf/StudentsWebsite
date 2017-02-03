using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.WebUI.Controllers
{
    public class StudentsController : Controller
    {
        IDataRepository dataRepository;

        public StudentsController(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        //
        // GET: /Students/
        public ActionResult Index()
        {
            User[] students = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Student).ToArray();
            Models.StudentViewModel[] models = new Models.StudentViewModel[students.Length];
            int subjectsCount;
            double averageRating;
            IEnumerable<Rating> ratings;

            for (int i = 0; i < students.Length; i++)
            {
                ratings = dataRepository.Ratings.Where(r => r.Student_UserName == students[i].UserName);
                subjectsCount = ratings.Count(r => r.Rate >= 0);
                if (subjectsCount > 0)
                    averageRating = ratings.Sum(r => r.Rate >= 0 ? (double)r.Rate : 0) / subjectsCount;
                else
                    averageRating = 0;
                
                
                models[i] = new Models.StudentViewModel()
                {
                    FullName = students[i].FirstName + " " + students[i].LastName,
                    UserName = students[i].UserName,
                    SubjectCount = subjectsCount,
                    AverageRating = (int)averageRating                     
                };
            }


                return View(models);
        }

        [Authorize(Roles = ("Lecturer, Dean, Student"))]
        public ActionResult Card(string userName = "")
        {
            Models.StudentEditViewModel editViewModel = new Models.StudentEditViewModel();

         
            User user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == Domain.Entities.User.Roles.Student);
            }

            if (user == null)
            {
                return RedirectToAction("Error");
            }

            editViewModel.Student = user;
            editViewModel.StudentUserName = user.UserName;
           
            Rating[] ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.UserName).ToArray();
            Models.StudentEditViewModel.LecturerSelection[] lecturerSelections = new Models.StudentEditViewModel.LecturerSelection[ratings.Length];
           
            User lecturer;
            for (int i = 0; i < ratings.Length; i++)
            {
                lecturer = dataRepository.GetUser(ratings[i].Lecturer_UserName);

                lecturerSelections[i] = new Models.StudentEditViewModel.LecturerSelection();
                lecturerSelections[i].LecturerUserName = ratings[i].Lecturer_UserName;
                lecturerSelections[i].LecturerFullName = lecturer.FirstName + " " + lecturer.LastName;
                lecturerSelections[i].LecturerSubject = lecturer.Subject;

                lecturerSelections[i].Rating = ratings[i].Rate;
                lecturerSelections[i].Selected = true;

            }

            editViewModel.Lecturers = lecturerSelections;

          

            return View(editViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Card(Models.StudentEditViewModel viewModel)
        {
            if (viewModel.StudentUserName != null && viewModel.StudentUserName.Trim() != "" &&
                viewModel.Lecturers != null &&
                (HttpContext.User.IsInRole("Dean") || HttpContext.User.IsInRole("Lecturer")))
            {

                Models.StudentEditViewModel.LecturerSelection[] Lecturers = viewModel.Lecturers;
                Rating[] newRatings = new Rating[Lecturers.Length];
                User Lecturer;

                for (int i = 0; i < Lecturers.Length; i++)
                {
                    Lecturer = dataRepository.GetUser(Lecturers[i].LecturerUserName);

                    if (Lecturer == null)
                    {
                        continue;
                    }

                    newRatings[i] = new Rating()
                    {
                        Lecturer_UserName = Lecturer.UserName,
                        Student_UserName = viewModel.StudentUserName,
                        Rate = Lecturers[i].Rating
                    };
                }

                dataRepository.SaveRatings(newRatings, viewModel.StudentUserName);

               
                

                 return RedirectToAction("Card", new { userName = viewModel.StudentUserName });
                
            }
            else
            {
                // Что-то не так со значениями данных
                return Card(viewModel.StudentUserName ?? viewModel.Student.UserName);
            }
        }

        [Authorize(Roles = ("Lecturer, Dean"))]
        public ActionResult Edit(string userName = "")
        {
            Models.StudentEditViewModel editViewModel = new Models.StudentEditViewModel();

            #region Загружаем пользователя
            User user = null;
            if (userName != "")
            {
                user = dataRepository.Users.FirstOrDefault(u => u.UserName == userName && u.Role == Domain.Entities.User.Roles.Student);
                ViewBag.ActionString = "Сохранить";
            }

            if (user == null)
            {
                //Random generation for testing purposes
                string firstName, lastName;
                Helpers.RandomDataGenerator.RandomName(out firstName, out lastName);
                user = new User()
                {

                    UserName = "",
                    FirstName = firstName,
                    LastName = lastName,
                    Role = Domain.Entities.User.Roles.Student,
                };

                ViewBag.ActionString = "Добавить";
            }
            editViewModel.Student = user;
            #endregion
            #region Список преподавателей, выбранных и не выбранных
            Rating[] ratings = dataRepository.Ratings.Where(r => r.Student_UserName == user.UserName).ToArray();
            User[] lecturers = dataRepository.Users.Where(u => u.Role == Domain.Entities.User.Roles.Lecturer).ToArray();
            Models.StudentEditViewModel.LecturerSelection[] lecturerSelections = new Models.StudentEditViewModel.LecturerSelection[lecturers.Length];
            Rating rating;
            for (int i = 0; i < lecturers.Length; i++)
            {
                lecturerSelections[i] = new Models.StudentEditViewModel.LecturerSelection();
                lecturerSelections[i].LecturerUserName = lecturers[i].UserName;
                lecturerSelections[i].LecturerFullName = lecturers[i].FirstName + " " + lecturers[i].LastName;
                lecturerSelections[i].LecturerSubject = lecturers[i].Subject;
                if ((rating = ratings.FirstOrDefault(r => r.Lecturer_UserName == lecturers[i].UserName)) != null)
                {
                    lecturerSelections[i].Rating = rating.Rate;
                    lecturerSelections[i].Selected = true;
                }
                else
                {
                    lecturerSelections[i].Rating = -1;
                    lecturerSelections[i].Selected = false;
                }
            }

            editViewModel.Lecturers = lecturerSelections;

            #endregion

                return View(editViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Dean, Lecturer")]
        public ActionResult Edit(Models.StudentEditViewModel editViewModel)
        {
            User user = editViewModel.Student;
            if (ModelState.IsValid)
            {
                user.Role = Domain.Entities.User.Roles.Student;
                dataRepository.SaveUser(user);

                HttpContext.User.IsInRole("Any");

                //Достаем список преподавателей-оценок (Ratings) из списка выбранных преподавателей 
                Models.StudentEditViewModel.LecturerSelection[] lecturers = editViewModel.Lecturers.Where(lect => lect.Selected).ToArray();
                Rating[] newRatings = new Rating[lecturers.Length];
                User lecturer;

                for (int i = 0; i < lecturers.Length; i++)
                {
                    lecturer = dataRepository.GetUser(lecturers[i].LecturerUserName);

                    if (lecturer == null)
                    {
                        continue;
                    }

                    newRatings[i] = new Rating()
                    {
                        Student_UserName = user.UserName,
                        Lecturer_UserName = lecturer.UserName,
                        Rate = lecturers[i].Rating
                    };
                }

                dataRepository.SaveRatings(newRatings, user.UserName);

                TempData["message"] = string.Format("Изменения пользователя \"{0} {1}\" были сохранены", user.FirstName, user.LastName);
                return RedirectToAction("Card", new { userName = editViewModel.Student.UserName });
            }
            else
            {
                // Что-то не так со значениями данных
                return Edit(editViewModel.StudentUserName);
            }

        }
	}
}