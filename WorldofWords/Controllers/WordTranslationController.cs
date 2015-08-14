using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;

namespace WorldofWords.Controllers
{
    [WowAuthorization(AllRoles = new[]{"Teacher", "Admin"})]
    [RoutePrefix("api/WordTranslation")]
    public class WordTranslationController : BaseController
    {
        private const int TranslationLanguageId = 4;
        private const int searchResultsCount = 10;

        private readonly IWordTranslationMapper wordTranslationMapper;
        private readonly IWordTranslationService wordTranslationService;
        private readonly IWordService wordService;
        private readonly IWordMapper wordMapper;

        public WordTranslationController(IWordTranslationService wordTranslationService, 
                                         IWordTranslationMapper wordTranslationMapper,
                                         IWordService wordService,
                                         IWordMapper wordMapper)
        {
            this.wordTranslationService = wordTranslationService;
            this.wordTranslationMapper = wordTranslationMapper;
            this.wordService = wordService;
            this.wordMapper = wordMapper;
        }

        [Route("ImportWordTranslations")]
        public List<WordTranslationModel> Post(List<WordTranslationImportModel> wordTranslations)
        {
            List<WordTranslationModel> wordTranslationsToReturn = new List<WordTranslationModel>();

            foreach (WordTranslationImportModel wordTranslation in wordTranslations)
            {
                wordTranslation.OriginalWordId = wordService.Exists(wordTranslation.OriginalWord, wordTranslation.LanguageId);

                if (wordTranslation.OriginalWordId == 0)
                {
                    wordTranslation.OriginalWordId = wordService.Add(wordMapper.ToDomainModel(
                        new WordModel()
                        {
                            Value = wordTranslation.OriginalWord,
                            LanguageId = wordTranslation.LanguageId,
                            Transcription = wordTranslation.Transcription,
                            Description = wordTranslation.Description
                        }));
                }

                wordTranslation.TranslationWordId = wordService.Exists(wordTranslation.TranslationWord, TranslationLanguageId);

                if (wordTranslation.TranslationWordId == 0)
                {
                    wordTranslation.TranslationWordId = wordService.Add(wordMapper.ToDomainModel(
                        new WordModel()
                        {
                            Value = wordTranslation.TranslationWord,
                            LanguageId = TranslationLanguageId
                        }));
                }

                int wordTranslationId = wordTranslationService.Exists(wordTranslation.OriginalWordId, wordTranslation.TranslationWordId);

                if (wordTranslationId == 0)
                {
                    wordTranslation.OwnerId = UserId;
                    wordTranslationId = wordTranslationService.Add(wordTranslationMapper.Map(wordTranslation));
                }

                wordTranslationsToReturn.Add(new WordTranslationModel()
                {
                    Id = wordTranslationId,
                    OriginalWord = wordTranslation.OriginalWord,
                    TranslationWord = wordTranslation.TranslationWord
                });
            }

            return wordTranslationsToReturn;
        }

        [Route("CreateWordTranslation")]
        public IHttpActionResult Post(WordTranslationImportModel wordtranslation)
        {
            if (wordtranslation == null)
                throw new ArgumentNullException("word translation model can't be empty");
            if(wordTranslationService.Exists(wordtranslation.OriginalWordId, wordtranslation.TranslationWordId)==0)
            {
                return Ok(wordTranslationService.Add(wordTranslationMapper.Map(wordtranslation)).ToString());
            }
            else
            {
                return BadRequest("Such wordtranslation exists");
            }
        }

        public List<WordTranslationModel> Get(string searchWord, int languageId)
        {
            if (searchWord == null)
            {
                throw new ArgumentNullException("searchWord", "Search word can't be null");
            }
            if (searchWord == String.Empty)
            {
                throw new ArgumentException("Search word can't be empty", "searchWord");
            }
            if (languageId <= 0)
            {
                throw new ArgumentException("Language ID can't be negative or 0", "languageId");
            }

            return wordTranslationMapper
                .MapRange(wordTranslationService.GetTopBySearchWord(searchWord, languageId, searchResultsCount));
        }

        public List<WordTranslationModel> Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID can't be negative or 0", "id");
            }
            return wordTranslationMapper.MapRange(wordTranslationService.GetByWordSuiteID(id));
        }
    }
}