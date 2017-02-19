using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data
{
    public class DbContext<T> : DbContext where T : BdEntity
    {
        public DbSet<T> DbSet { get; protected set; }
    }

    public class EfdbContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }

        public Dictionary<Type, Func<object>> dbSetGetters = new Dictionary<Type, Func<object>>();

        public EfdbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EfdbContext>());
            dbSetGetters.Add(typeof(DbUser), () => Users);
            dbSetGetters.Add(typeof(Rating), () => Ratings);
            dbSetGetters.Add(typeof(Lecturer), () => Lecturers);
            dbSetGetters.Add(typeof(Student), () => Students);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
             * modelBuilder.Entity<Card>()
                    .HasRequired(c => c.Stage)
                    .WithMany()
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Side>()
                    .HasRequired(s => s.Stage)
                    .WithMany()
                    .WillCascadeOnDelete(false);
                    */

            modelBuilder.Entity<Rating>()
                .HasRequired(r => r.Student)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Rating>()
                .HasRequired(r => r.Lecturer)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<T> GetDbSet<T>() where T : BdEntity
        {
            if (dbSetGetters.ContainsKey(typeof(T)))
                return (DbSet<T>) dbSetGetters[typeof(T)]();

            throw new ArgumentException($"There is no dbSet for type {typeof(T)}");
        }

        
    }
}