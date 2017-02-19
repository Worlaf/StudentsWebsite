using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.Data.Services
{
    public interface IRatingService : IDbEntityService<Rating>
    {
        
    }
    public class RatingService : IRatingService
    {
        public void Save(Rating entity)
        {
            throw new NotImplementedException();
        }
    }
}
