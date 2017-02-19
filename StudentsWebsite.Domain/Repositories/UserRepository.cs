using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Repositories
{
    public interface IDbUserRepository : IDataRepository<DbUser>
    {
        DbUser ByEmail(string email, bool throwExceptionIfNotFound = false);
    }
    public class DbUserRepository : DataRepositoryBase<Entities.DbUser>, IDbUserRepository
    {
        public DbUser ByEmail(string email, bool throwExceptionIfNotFound = false)
        {
            return Single(u => u.Email == email);
        }
    }
}
