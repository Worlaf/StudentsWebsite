﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsWebsite.Data.Entities;
using StudentsWebsite.Data.Abstract;


namespace StudentsWebsite.Data.Concrete
{
    public class EFDataRepository : IDataRepositoryOld
    {
        protected EfdbContext context = new EfdbContext();

        public IEnumerable<Rating> Ratings
        {
            get { return context.Ratings; }
        }

        public IEnumerable<DbUser> Users
        {
            get { return context.Users; }
        }

        public void SaveUser(DbUser user)
        {
            if (!UserNameExists(user.Email))
                context.Users.Add(user);
            else
            {
                var dbEntry = context.Users.Find(user.Email);
                if (dbEntry != null)
                {
                    dbEntry.FirstName = user.FirstName;
                    dbEntry.LastName = user.LastName;
                    dbEntry.Subject = user.Subject;  
                    dbEntry.Password = user.Password;
                }
            }

            context.SaveChanges();
        }

        public bool UserNameExists(string username)
        {
            return GetUser(username) != null;
        }

        public DbUser GetUser(string username)
        {
            return context.Users.Find(username);
        }

        public void SaveRatings(IEnumerable<Rating> ratings)
        {
            Rating rate;
            foreach (Rating r in ratings)
            {
                rate = context.Ratings.FirstOrDefault(rat => rat.Lecturer_UserName == r.Lecturer_UserName && rat.Student_UserName == r.Student_UserName);
                if ((rate) != null)
                    rate.Rate = r.Rate;
                else if (ratings.Count(rat => rat.Lecturer_UserName == r.Lecturer_UserName && rat.Student_UserName == r.Student_UserName) <= 1)
                    context.Ratings.Add(r);
            }
            
            context.SaveChanges();
        }
        public void SaveRatings(IEnumerable<Rating> ratings, string forUserName)
        {
            Rating rate;
            var existedRatings = context.Ratings.Where(r => r.Lecturer_UserName == forUserName || r.Student_UserName == forUserName);

            //Добавляем не существующие подписки студентов на предметы и обновляем существующие
            foreach (Rating r in ratings)
            {
                rate = existedRatings.FirstOrDefault(rat => rat.Lecturer_UserName == r.Lecturer_UserName && rat.Student_UserName == r.Student_UserName);
                if ((rate) != null)
                    rate.Rate = r.Rate;
                else
                    context.Ratings.Add(r);
            }

            //Удаляем подписки (оценки), отсутствующие в пришедшем списке
            foreach (Rating r in existedRatings)
            {
                rate = ratings.FirstOrDefault(rat => rat.Lecturer_UserName == r.Lecturer_UserName && rat.Student_UserName == r.Student_UserName);
                if ((rate) == null)
                    context.Ratings.Remove(r);
            }

            context.SaveChanges();
        }

    }
}
