using System.Data.Entity;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF.Contracts;

namespace WorldOfWords.Infrastructure.Data.EF
{
    public class CourseRepository : EFRepository<Course>, ICourseRepository
    {
        public CourseRepository(DbContext dbContext) : base(dbContext) { }
    }
}
