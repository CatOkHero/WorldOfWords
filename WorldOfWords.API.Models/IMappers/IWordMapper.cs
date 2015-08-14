using WorldOfWords.API.Models.Models;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface IWordMapper
    {
        Word ToDomainModel(WordModel apiModel);
        WordModel ToApiModel(Word domainModel);
    }
}
