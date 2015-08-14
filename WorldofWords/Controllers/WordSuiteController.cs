using System;
using System.Collections.Generic;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;

namespace WorldofWords.Controllers
{
    [RoutePrefix("api/wordsuite")]
    public class WordSuiteController : BaseController
    {
        private readonly IWordSuiteService wordSuiteService;
        private readonly IWordSuiteMapper wordSuiteMapper;
        private readonly IWordProgressMapper wordProgressMapper;
        private readonly ITrainingWordSuiteMapper trainingWordSuiteMapper;
        private readonly IWordProgressService wordProgressService;

        public WordSuiteController(
            IWordSuiteService wordSuiteService,
            IWordSuiteMapper wordSuiteMapper,
            ITrainingWordSuiteMapper trainingWordSuiteMapper,
            IWordProgressService wordProgressService,
            IWordProgressMapper wordProgressMapper)
        {
            this.wordSuiteMapper = wordSuiteMapper;
            this.wordSuiteService = wordSuiteService;
            this.trainingWordSuiteMapper = trainingWordSuiteMapper;
            this.wordProgressMapper = wordProgressMapper;
            this.wordProgressService = wordProgressService;
        }

        public WordSuiteController() { }

        [WowAuthorization(Roles = "Teacher")]
        [Route("CreateWordSuite")]
        public IHttpActionResult Post(WordSuiteModel wordSuite)
        {
            if (wordSuite == null)
            {
                throw new ArgumentNullException("wordSuite", "WordSuite can't be null");
            }

            int wordSuiteId = wordSuiteService.Add(wordSuiteMapper.Map(wordSuite));

            if (wordSuiteId <= 0)
            {
                return BadRequest("Failed to add WordSuite");
            }

            if (wordSuite.WordTranslationsId.Count > 0)
            {
                if (!wordProgressService.AddRange(wordProgressMapper.MapRange(wordSuiteId, wordSuite.WordTranslationsId)))
                {
                    return BadRequest("Failed to add WordTranslations");
                }
            }

            return Ok();
        }

        [WowAuthorization(Roles = "Teacher")]
        [Route("EditWordSuite")]
        public IHttpActionResult Post(WordSuiteEditModel wordSuite)
        {
            if (wordSuite == null)
            {
                throw new ArgumentNullException("wordSuite", "WordSuite can't be null");
            }

            if (wordSuite.IsBasicInfoChanged)
            {
                if (!wordSuiteService.Update(wordSuiteMapper.Map(wordSuite)))
                {
                    return BadRequest("Failed to edit WordSuite");
                }
            }

            if (wordSuite.WordTranslationsToDeleteIdRange != null)
            {
                if (wordSuite.WordTranslationsToDeleteIdRange.Count > 0)
                {
                    if (!wordProgressService.RemoveRange(wordProgressMapper.MapRange(wordSuite.Id, wordSuite.WordTranslationsToDeleteIdRange)))
                    {
                        return BadRequest("Failed to remove WordTranslations");
                    }
                }
            }

            if (wordSuite.WordTranslationsToAddIdRange != null)
            {
                if (wordSuite.WordTranslationsToAddIdRange.Count > 0)
                {
                    if (!wordProgressService.AddRange(wordProgressMapper.MapRange(wordSuite.Id, wordSuite.WordTranslationsToAddIdRange)))
                    {
                        return BadRequest("Failed to add WordTranslations");
                    }
                }
            }

            return Ok();
        }

        [Route("GetTeacherWordSuites")]
        public List<WordSuiteEditModel> GetTeacherWordSuites(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id", "ID can't be negative or 0");
            }
            return wordSuiteMapper.MapRangeForEdit(wordSuiteService.GetTeacherWordSuites(id));
        }

        [Route("GetWordSuitesByLanguageId")]
        public List<CourseWordSuiteModel> GetWordSuites(int languageId)
        {
            if (languageId <= 0)
            {
                throw new ArgumentNullException("languageID", "Language ID can't be negative or 0");
            }

            return wordSuiteMapper.Map(wordSuiteService.GetWordSuitesByOwnerAndLanguageId(UserId, languageId));
        }

        [Route("GetWordSuiteByID")]
        public WordSuiteEditModel GetByID(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id", "ID can't be negative or 0");
            }

            return wordSuiteMapper.MapForEdit(wordSuiteService.GetByID(id));
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("id", "ID can't be negative or 0");
            }

            return wordSuiteService.Delete(id) ? Ok() as IHttpActionResult : BadRequest() as IHttpActionResult;
        }

        [Route("AddWordProgresses")]
        public IHttpActionResult AddWordProgresses(List<WordProgressModel> wordProgresses)
        {
            if (wordProgresses == null)
                throw new ArgumentNullException("wordProgresses", "WordProgresses can't be null");

            return wordProgressService.AddRange(wordProgressMapper.MapRange(wordProgresses))
                ? Ok() as IHttpActionResult
                : BadRequest() as IHttpActionResult;
        }

        [Route("RemoveWordProgresses")]
        public IHttpActionResult RemoveWordProgress(WordProgressModel wordProgress)
        {
            if (wordProgress == null)
                throw new ArgumentNullException("wordProgress", "WordProgress can't be null");

            return wordProgressService.RemoveByStudent(wordProgressMapper.Map(wordProgress))
                ? Ok() as IHttpActionResult
                : BadRequest() as IHttpActionResult;
        }
    }
}
