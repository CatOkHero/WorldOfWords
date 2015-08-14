using System.Data.Entity;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF.Contracts;

namespace WorldOfWords.Infrastructure.Data.EF
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }
        public IQueryable<string> GetDistinctNames()
        {
            return DbContext.Set<User>().Select(u => u.Name).Distinct();
        }
    }
}
