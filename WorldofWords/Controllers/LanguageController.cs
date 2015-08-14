using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WorldOfWords.API.Models.IMappers;
using WorldOfWords.API.Models.Models;
using WorldOfWords.Domain.Services.IServices;

namespace WorldofWords.Controllers
{
   
    /// <summary>
    /// Responsible for obtaining and manipulating a list of languages.
    /// </summary>
    [WowAuthorization(AllRoles = new[] { "Teacher", "Student" })]
    public class LanguageController : ApiController
    {
        private readonly ILanguageMapper _mapper;
        private readonly ILanguageService _service;

        public LanguageController(ILanguageService service, ILanguageMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all languages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageModel> Get()
        {
            return _service
                .GetAllLanguages()
                .Select(l => _mapper.ToApiModel(l));
        }

        /// <summary>
        /// Adds a new language to the database.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public int Post(LanguageModel language)
        {
            if (language == null)
            {
                throw new ArgumentException("Language cannot be null!", "language");
            }
            return _service.AddLanguage(_mapper.ToDomainModel(language));
        }

        /// <summary>
        /// Removes language from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id", "Language ID can't be negative or 0");
            }

            return _service.RemoveLanguage(id)
                ? Ok() as IHttpActionResult
                : BadRequest() as IHttpActionResult;
        }
    }
}