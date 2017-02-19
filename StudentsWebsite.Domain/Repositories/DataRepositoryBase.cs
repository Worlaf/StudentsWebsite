using System;
using System.Linq;
using System.Linq.Expressions;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Repositories
{
    public interface IDataRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector);

        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);

        IQueryable<TResult> GetMany<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector);

        TEntity Single(Expression<Func<TEntity, bool>> where);
        TResult Single<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector);
        TEntity First(Expression<Func<TEntity, bool>> where);
        TResult First<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector);

        void Save(TEntity entity);
    }
    public abstract class DataRepositoryBase<TEntity> : IDataRepository<TEntity> where TEntity : Entities.BdEntity
    {
        protected EfdbContext context = new EfdbContext();
        public virtual IQueryable<TEntity> GetAll()
        {
            return GetDbSet();
        }

        public System.Data.Entity.DbSet<TEntity> GetDbSet()
        {
            return context.GetDbSet<TEntity>();
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

        public TEntity Single(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().Single(where);
        }

        public TResult Single<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where, selector).Single();
        }

        public TEntity First(Expression<Func<TEntity, bool>> where)
        {
            return GetAll().First(where);
        }

        public TResult First<TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TResult>> selector)
        {
            return GetMany(where, selector).First();
        }
        public void Save(TEntity entity)
        {
            var entry = GetDbSet().SingleOrDefault(e => e.Id == entity.Id);

            if (entry == null)
                GetDbSet().Add(entity);
            else
                entry = entity;

            context.SaveChanges();
        }
    }
}
