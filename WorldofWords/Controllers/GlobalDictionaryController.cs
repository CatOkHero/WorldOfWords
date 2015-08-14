using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;

namespace WorldofWords.Controllers
{
    [WowAuthorization(AllRoles = new[] { "Teacher", "Student", "Admin" })]
    public class GlobalDictionaryController : ApiController
    {
        private readonly IWordTranslationMapper wordTranslationMapper;
        private readonly IWordTranslationService wordTranslationService;

        public GlobalDictionaryController(IWordTranslationMapper _wordTranslationMapper, IWordTranslationService _wordTranslationService)
        {
            wordTranslationMapper = _wordTranslationMapper;
            wordTranslationService = _wordTranslationService;
        }

        public  List<WordTranslationImportModel> Get(int start, int end, int language)
        {
            return wordTranslationService.GetWordsFromInterval(start, end, language).Select(item => wordTranslationMapper.MapToImportModel(item)).ToList();
        }
 
        public int Get(int languageID)
        {
            return wordTranslationService.GetAmountOfWordTranslationsByLanguage(languageID);
        }

        public int GetAmountOfWordsBySearchValue(string searchValue, int languageId)
        {
            return wordTranslationService.GetAmountOfWordsBySearchValues(searchValue, languageId);
        }

        public List<WordTranslationModel> GetBySearchValue(string searchValue, int startOfInterval, int endOfInterval, int languageId)
        {
            return wordTranslationService.GetWordsWithSearchValue(searchValue, startOfInterval, endOfInterval, languageId)
                .Select(item => wordTranslationMapper.Map(item))
                .ToList();
        }
    }
}
