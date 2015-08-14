using System.Data.Entity;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF.Contracts;

namespace WorldOfWords.Infrastructure.Data.EF
{
    public class WordSuiteRepository: EFRepository<WordSuite>, IWordSuiteRepository
    {
        public WordSuiteRepository(DbContext dbContext) : base(dbContext) { }
    }
}
