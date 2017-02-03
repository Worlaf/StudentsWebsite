using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.Domain.Abstract;

namespace StudentsWebsite.WebUI.Helpers
{
    public static class TestDataTableGenerator
    {
        public static void FillTable(IDataRepository repository)
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
            User user;

            user = new User(){
                    UserName = "dean",
                    Password = "dean",
                    FirstName = "Владимир",
                    LastName = "Петров",
                    Subject = "",
                    Role = User.Roles.Dean
                };
            repository.SaveUser(user);
            user = new User()
            {
                UserName = "student",
                Password = "student",
                FirstName = "Семён",
                LastName = "Полупроводников",
                Subject = "",
                Role = User.Roles.Student
            };
            repository.SaveUser(user);
            user = new User()
            {
                UserName = "lecturer",
                Password = "lecturer",
                FirstName = "Дарья",
                LastName = "Задушевная",
                Subject = "Психология",
                Role = User.Roles.Lecturer
            };
            repository.SaveUser(user);

            for (i = 0; i < subjects.Length; i++)
            {
                pass = RandomDataGenerator.RandomPassword(4);
                while(repository.UserNameExists(login = RandomDataGenerator.RandomPassword(4)));
                RandomDataGenerator.RandomName(out firstName, out lastName);
                subject = subjects[i];

                user = new User(){
                    UserName = login,
                    Password = pass,
                    FirstName = firstName,
                    LastName = lastName,
                    Subject = subject,
                    Role = User.Roles.Lecturer
                };

                repository.SaveUser(user);
            }


            for (i = 0; i < 20; i++)
            {
                pass = RandomDataGenerator.RandomPassword(4);
                while (repository.UserNameExists(login = RandomDataGenerator.RandomPassword(4))) ;
                RandomDataGenerator.RandomName(out firstName, out lastName);
               

                user = new User()
                {
                    UserName = login,
                    Password = pass,
                    FirstName = firstName,
                    LastName = lastName,                    
                    Role = User.Roles.Student
                };

                repository.SaveUser(user);
            }

            //Генерация посещений студентов и оценок
            
            User[] students = repository.Users.Where(u => u.Role == User.Roles.Student).ToArray();
            User[] lecturers = repository.Users.Where(u => u.Role == User.Roles.Lecturer).ToArray();
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
                        Student_UserName = students[index].UserName,
                        Lecturer_UserName = lecturers[i].UserName,
                        Rate = rnd.Next(-1, 100)
                    };
                    ratings.Add(rating);
                    
                }
            }
            repository.SaveRatings(ratings);
            
        }
    }
}