using System.Collections.Generic;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;
using System.Linq;
using System;

namespace WorldofWords.Controllers
{
    [WowAuthorization(Roles = "Student")]
    public class WordsController : ApiController
    {
        private readonly IWordMapper _mapper;
        private readonly IWordService _service;
        
        public WordsController(IWordMapper mapper, IWordService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public WordModel GetWordById(int wordId)
        {
            return _mapper.ToApiModel(_service.GetWordById(wordId));
        }

        public IEnumerable<WordModel> Get(int languageId)
        {
            return _service
                .GetAllWordsBySpecificLanguage(languageId)
                .Select(l => _mapper.ToApiModel(l));
        }

        public IEnumerable<WordModel> Get()
        {
            return _service
                .GetAllWords()
                .Select(l => _mapper.ToApiModel(l));
        }

        public List<WordModel> Get(string searchWord, int languageId, int searchResultsCount)
        {
            return _service.GetTopBySearchWord(searchWord, languageId, searchResultsCount).Select(item => _mapper.ToApiModel(item)).ToList();
        }

        public int Post(WordModel word)
        {
            if (word == null)
                throw new System.ArgumentNullException("empty word model");
            return _service.Add(_mapper.ToDomainModel(word));
        }
    }
}
