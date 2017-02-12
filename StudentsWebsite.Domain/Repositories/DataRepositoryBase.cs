using System;
using System.Linq;
using System.Linq.Expressions;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.Domain.Repositories
{
    public interface IDataRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector);

        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);

        IQueryable<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector);

        void Save(TEntity entity);
    }
    public abstract class DataRepositoryBase<TEntity> : IDataRepository<TEntity> where TEntity : Entities.TEntity
    {
        protected DbContext<TEntity> DbContext = new DbContext<TEntity>();
        public IQueryable<TEntity> GetAll()
        {
            return DbContext.DbSet;
        }

        public IQueryable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Select(selector);
        }
        
        public IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().Where(where);
        }

        public IQueryable<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Where(where).Select(selector);
        }
        public void Save(TEntity entity)
        {
            var entry = DbContext.DbSet.SingleOrDefault(e => e.Id == entity.Id);

            if (entry == null)
                DbContext.DbSet.Add(entity);
            else
                entry = entity;

            DbContext.SaveChanges();
        }
    }
}
