using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;
using System.Linq;
using System;

namespace WorldofWords.Controllers
{
    [CustomAuthorization(Roles = "Student")]
    public class TestWordSuiteController : BaseController
    {
        private readonly IQuizWordSuiteMapper _quizMapper;
        private readonly ITrainingWordSuiteMapper _trainingMapper;
        private readonly IWordProgressService _progressService;
        private readonly IWordSuiteService _service;

        public TestWordSuiteController(IQuizWordSuiteMapper quizMapper, ITrainingWordSuiteMapper trainingMapper, IWordSuiteService service,
            IWordProgressService progressService)
        {
            _quizMapper = quizMapper;
            _trainingMapper = trainingMapper;
            _service = service;
            _progressService = progressService;
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public IHttpActionResult GetTask(int id)
        {
            var suite = _quizMapper.Map(_service.GetWithNotStudiedWords(id));

            if (suite.OwnerId == UserId)
            {
                if (suite.QuizStartTime == null)
                {
                    suite.QuizStartTime = new DateTime();
                }

                if (((DateTime)suite.QuizStartTime).AddDays(1) < DateTime.Now)
                {
                    _service.SetTime(suite.Id);
                    Shuffle(suite.WordTranslations);
                    return Ok(suite);
                }

                return BadRequest("Sorry, only one quiz per day!");
            }

            return BadRequest("You can`t write not your courses!");
        }

        public IHttpActionResult Check(TrainingWordSuiteModel data)
        {
            TrainingWordSuiteModel wordSuite = _trainingMapper.Map(_service.GetWithNotStudiedWords(data.Id));
            DateTime EndTime = wordSuite.QuizStartTime.Value.AddSeconds(wordSuite.QuizResponseTime
                * wordSuite.WordTranslations.Count + 20);
            if (EndTime > DateTime.Now)
            {
                for (int i = 0; i < data.WordTranslations.Count; i++)
                {
                    data.WordTranslations[i].OriginalWord = wordSuite.WordTranslations.First(x => x.Id == data.WordTranslations[i].Id).OriginalWord;
                    if (data.WordTranslations[i].TranslationWord == data.WordTranslations[i].OriginalWord)
                    {
                        data.WordTranslations[i].Result = true;
                        _progressService.IncrementProgress(data.Id, data.WordTranslations[i].Id);
                    }
                }
                return Ok(data);
            }
            return BadRequest("Don`t cheat!");
        }
    }
}
