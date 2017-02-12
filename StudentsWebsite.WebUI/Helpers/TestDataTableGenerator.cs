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
                    UserName = "dean",
                    Password = "dean",
                    FirstName = "Владимир",
                    LastName = "Петров",
                    Subject = "",
                    Role = UserRoles.Dean
                };
            repository.SaveUser(user);
            user = new DbUser()
            {
                UserName = "student",
                Password = "student",
                FirstName = "Семён",
                LastName = "Полупроводников",
                Subject = "",
                Role = UserRoles.Student
            };
            repository.SaveUser(user);
            user = new DbUser()
            {
                UserName = "lecturer",
                Password = "lecturer",
                FirstName = "Дарья",
                LastName = "Задушевная",
                Subject = "Психология",
                Role = UserRoles.Lecturer
            };
            repository.SaveUser(user);

            for (i = 0; i < subjects.Length; i++)
            {
                pass = RandomDataGenerator.RandomPassword(4);
                while(repository.UserNameExists(login = RandomDataGenerator.RandomPassword(4)));
                RandomDataGenerator.RandomName(out firstName, out lastName);
                subject = subjects[i];

                user = new DbUser(){
                    UserName = login,
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
                pass = RandomDataGenerator.RandomPassword(4);
                while (repository.UserNameExists(login = RandomDataGenerator.RandomPassword(4))) ;
                RandomDataGenerator.RandomName(out firstName, out lastName);
               

                user = new DbUser()
                {
                    UserName = login,
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