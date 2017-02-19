using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Repositories
{
    public interface IRatingRepository : IDataRepository<Rating>
    {
        Rating ByStudentLecturerPair(Guid studendId, Guid lecturerId);

        IQueryable<Rating> ByLecturer(Guid lecturerId);
        IQueryable<TResult> ByLecturer<TResult>(Guid lecturerId, Expression<Func<Rating, TResult>> selector);
        IQueryable<Rating> ByStudent(Guid studentId);
        IQueryable<TResult> ByStudent<TResult>(Guid studentId, Expression<Func<Rating, TResult>> selector);
    }
    public class RatingRepository:DataRepositoryBase<Entities.Rating>, IRatingRepository
    {
        public Rating ByStudentLecturerPair(Guid studendId, Guid lecturerId)
        {
            return Single(r => r.StudentId == studendId && r.LecturerId == lecturerId);
        }

        public IQueryable<Rating> ByLecturer(Guid lecturerId)
        {
            return GetMany(r => r.LecturerId == lecturerId);
        }

        public IQueryable<TResult> ByLecturer<TResult>(Guid lecturerId, Expression<Func<Rating, TResult>> selector)
        {
            return GetMany(r => r.LecturerId == lecturerId, selector);
        }

        public IQueryable<Rating> ByStudent(Guid studentId)
        {
            return GetMany(r => r.StudentId == studentId);
        }

        public IQueryable<TResult> ByStudent<TResult>(Guid studentId, Expression<Func<Rating, TResult>> selector)
        {
            return GetMany(r => r.StudentId == studentId, selector);
        }
    }
}
