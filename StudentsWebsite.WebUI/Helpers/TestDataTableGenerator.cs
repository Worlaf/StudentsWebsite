using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Ninject;
using StudentsWebsite.Data;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Abstract;
using StudentsWebsite.Data.Repositories;
using StudentsWebsite.Data.Services;

namespace StudentsWebsite.WebUI.Helpers
{
    public class TestDataTableGenerator
    {
        [Inject]
        public IDbUserService DbUserService { get; set; }
        [Inject]
        public IStudentService StudentService { get; set; }
        [Inject]
        public ILecturerService LecturerService { get; set; }
        [Inject]
        public IStudentRepository StudentRepository { get; set; }
        [Inject]
        public ILecturerRepository LecturerRepository { get; set; }
        [Inject]
        public IDbUserRepository DbUserRepository { get; set; }
        [Inject]
        public IRatingService RatingService { get; set; }
        [Inject]
        public IRatingRepository RatingRepository { get; set; }


        public void Generate(int studentsCount, int lecturersCount)
        {
            var students = GenerateStudents(studentsCount);
            var lecturers = GenerateLecturers(lecturersCount);
            GenerateDean("dean@mail.com");

            GenerateRatings(students.ToArray(), lecturers.ToArray());
        }

        public void GenerateRatings(Student[] students, Lecturer[] lecturers, int minStudentsCount = 0, int maxStudentsCount = 10)
        {
            var rnd = new Random();
            maxStudentsCount = students.Length < maxStudentsCount ? students.Length : maxStudentsCount;
            
            foreach (var lecturer in lecturers)
            {
                var studentsCount = rnd.Next(minStudentsCount, maxStudentsCount);

                for (var i = minStudentsCount; i < studentsCount; i++)
                {
                    var student = students[rnd.Next(students.Length - 1)];

                    var rating = new Rating
                    {
                        StudentId = student.Id,
                        LecturerId = lecturer.Id,
                        Rate = null
                    };

                    RatingService.Save(rating);
                }
            }
        }

        public DbUser GenerateDean(string email = null)
        {
            var user = GenerateUser(UserRoles.Dean);

            if (!email.IsEmpty())
                user.Email = email;

            DbUserService.Save(user);

            return DbUserRepository.ByEmail(user.Email);
        }

        public IEnumerable<Lecturer> GenerateLecturers(int count)
        {
            if (LecturerService == null)
                LecturerService = DependencyResolver.Current.GetService<IKernel>().Get<ILecturerService>();

            for (var i = 0; i < count; i++)
            {
                var lecturer = GenerateLecturer();
                LecturerService.Save(lecturer);

                yield return LecturerRepository.ByEmail(lecturer.User.Email);
            }
        }

        public IEnumerable<Student> GenerateStudents(int count)
        {
            if (StudentService == null)
                StudentService = DependencyResolver.Current.GetService<IKernel>().Get<IStudentService>();

            for (var i = 0; i < count; i++)
            {
                var student = GenerateStudent();
                StudentService.Save(student);

                yield return StudentRepository.ByEmail(student.User.Email);
            }
        }

        private Lecturer GenerateLecturer()
        {
            var user = GenerateUser(UserRoles.Lecturer);
            return new Lecturer
            {
                User = user,
                Subject = RandomDataGenerator.NextSubject()
            };
        }

        private Student GenerateStudent()
        {
            var user = GenerateUser(UserRoles.Student);
            return new Student
            {
                User = user,
                AverageRating = null
            };
        }

        private DbUser GenerateUser(UserRoles role)
        {
            string firstName, lastName;
            RandomDataGenerator.RandomName(out firstName, out lastName);
            return new DbUser
            {
                FirstName = firstName,
                LastName = lastName,
                Password = "123",
                Role = role,
                Email = RandomDataGenerator.RandomEmail()
            };
        }
        
        public static void FillTable(IDataRepositoryOld repository)
        {
            int i, j;
            if (repository.Users.Count() != 0)
                return;

            string[] subjects = new string[] {
                "Математический анализ", "Линейная алгебра" ,"Аналитическая геометрия", "Теория автоматики и управления", "Сигналы и системы",
                "Физика", "Спецглавы физики", "Термодинамика", "Сопротивление материалов", "Физика плазмы", "Квантовая физика",
                "Общая химия", "Органическая химия", "Химия расплавов", 
                "Электротехника", "Переходные процессы в электронных устройствах", "Электроника", "Микроконтроллеры",
                "Программирование C++", "Программирование Java", "Программирование C#", "Низкоуровневое программирование",
                "История", "Отечественная история", "Экономика", "Правоведение",
                 "Английский язык", "Немецкий язык", "Китайский язык", "Японский язык", "Корейский язык",
                "Стандартизация", "Метрология"
            };
            string login, pass, firstName, lastName, subject;
            DbUser user;

            user = new DbUser(){
                    Email = "dean",
                    Password = "dean",
                    FirstName = "Владимир",
                    LastName = "Петров",
                    Subject = "",
                    Role = UserRoles.Dean
                };
            repository.SaveUser(user);
            user = new DbUser()
            {
                Email = "student",
                Password = "student",
                FirstName = "Семён",
                LastName = "Полупроводников",
                Subject = "",
                Role = UserRoles.Student
            };
            repository.SaveUser(user);
            user = new DbUser()
            {
                Email = "lecturer",
                Password = "lecturer",
                FirstName = "Дарья",
                LastName = "Задушевная",
                Subject = "Психология",
                Role = UserRoles.Lecturer
            };
            repository.SaveUser(user);

            for (i = 0; i < subjects.Length; i++)
            {
                pass = RandomDataGenerator.RandomString(4);
                while(repository.UserNameExists(login = RandomDataGenerator.RandomString(4)));
                RandomDataGenerator.RandomName(out firstName, out lastName);
                subject = subjects[i];

                user = new DbUser(){
                    Email = login,
                    Password = pass,
                    FirstName = firstName,
                    LastName = lastName,
                    Subject = subject,
                    Role = UserRoles.Lecturer
                };

                repository.SaveUser(user);
            }


            for (i = 0; i < 20; i++)
            {
                pass = RandomDataGenerator.RandomString(4);
                while (repository.UserNameExists(login = RandomDataGenerator.RandomString(4))) ;
                RandomDataGenerator.RandomName(out firstName, out lastName);
               

                user = new DbUser()
                {
                    Email = login,
                    Password = pass,
                    FirstName = firstName,
                    LastName = lastName,                    
                    Role = UserRoles.Student
                };

                repository.SaveUser(user);
            }

            //Генерация посещений студентов и оценок
            
            DbUser[] students = repository.Users.Where(u => u.Role == UserRoles.Student).ToArray();
            DbUser[] lecturers = repository.Users.Where(u => u.Role == UserRoles.Lecturer).ToArray();
            Random rnd = new Random();
            int count, index;
            Rating rating;
            List<Rating> ratings = new List<Rating>();

            for (i = 0; i < lecturers.Length; i++)
            {
                count = rnd.Next(1, students.Length - 1);
                for (j = 0; j < count; j++)
                {
                    index = rnd.Next(students.Length - 1);
                    rating = new Rating()
                    {
                        Student_UserName = students[index].Email,
                        Lecturer_UserName = lecturers[i].Email,
                        Rate = rnd.Next(-1, 100)
                    };
                    ratings.Add(rating);
                    
                }
            }
            repository.SaveRatings(ratings);
            
        }
    }
}