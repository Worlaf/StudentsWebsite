using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Services
{
    public interface IDbEntityService<in TEntity> where TEntity : BdEntity
    {
        void Save(TEntity entity);
    }
}
