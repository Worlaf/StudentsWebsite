using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StudentsWebsite.Domain.Entities;

namespace StudentsWebsite.Domain
{
    public class DbContext<T> : DbContext where T : TEntity
    {
        public DbSet<T> DbSet { get; protected set; }
    }

    public class EfdbContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}