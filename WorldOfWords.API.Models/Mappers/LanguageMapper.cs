using WorldOfWords.API.Models.IMappers;
using WorldOfWords.API.Models.Models;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public class LanguageMapper : ILanguageMapper
    {
        public Language ToDomainModel(LanguageModel apiModel)
        {
            Language result = new Language
            {
                Name = apiModel.Name
            };
            if (apiModel.Id != null)
            {
                result.Id = (int)apiModel.Id;
            }
            return result;
        }

        public LanguageModel ToApiModel(Language domainModel)
        {
            LanguageModel result = new LanguageModel
            {
                Id = domainModel.Id,
                Name = domainModel.Name
            };
            return result;
        }
    }
}
