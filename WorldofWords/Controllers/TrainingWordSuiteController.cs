using System.Collections.Generic;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Services;
using System.Linq;
using System;
using WorldOfWords.Domain.Services.MessagesAndConsts;

namespace WorldofWords.Controllers
{
    [WowAuthorization(Roles = "Student")]
    [RoutePrefix("api/TrainingWordSuite")]

    public class TrainingWordSuiteController : BaseController
    {
        private readonly IQuizWordSuiteMapper _quizMapper;
        private readonly ITrainingWordSuiteMapper _trainingMapper;
        private readonly IWordProgressService _progressService;
        private readonly IWordSuiteService _service;
        private readonly IWordProgressMapper _progressMapper;

        public TrainingWordSuiteController(IQuizWordSuiteMapper quizMapper, ITrainingWordSuiteMapper trainingMapper, IWordSuiteService service,
            IWordProgressService progressService, IWordProgressMapper progressMapper)
        {
            _quizMapper = quizMapper;
            _trainingMapper = trainingMapper;
            _service = service;
            _progressService = progressService;
            _progressMapper = progressMapper;
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Random random = new Random();
            var currentNode = list.Count;
            while (currentNode > 1)
            {
                currentNode--;
                var randomNode = random.Next(currentNode + 1);
                T value = list[randomNode];
                list[randomNode] = list[currentNode];
                list[currentNode] = value;
            }
        }

        [Route("AllWords")]
        public TrainingWordSuiteModel GetWordSuiteWithAllWords(int id)
        {
            var wordSuite = _trainingMapper.Map(_service.GetByID(id));
            foreach (WordTranslationModel word in wordSuite.WordTranslations)
            {
                if (_progressService.IsStudentWord(_progressMapper.Map(wordSuite.Id, word.Id)))
                {
                    word.IsStudentWord = true;
                }
                else
                {
                    word.IsStudentWord = false;
                }
            }
            return wordSuite;
        }

        [Route("NotStudiedWords")]
        public TrainingWordSuiteModel GetWordSuiteWithNotStudiedWords(int id)
        {
            return _trainingMapper.Map(_service.GetWithNotStudiedWords(id));
        }

        [Route("Task")]
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
                return BadRequest(MessagesContainer.OneQuizPerDay);
            }
            return BadRequest(MessagesContainer.NotYourQuiz);
        }

        public IHttpActionResult CheckTask(TrainingWordSuiteModel answer)
        {
            TrainingWordSuiteModel wordSuite = _trainingMapper.Map(_service.GetWithNotStudiedWords(answer.Id));
            DateTime endTime = wordSuite.QuizStartTime.Value.
                AddSeconds(wordSuite.QuizResponseTime * wordSuite.WordTranslations.Count + 20);
            if (endTime > DateTime.Now)
            {
                Check(answer, wordSuite);
                return Ok(answer);
            }
            return BadRequest(MessagesContainer.TimeCheating);
        }

        private void Check(TrainingWordSuiteModel answer, TrainingWordSuiteModel wordSuite)
        {
            foreach (var word in answer.WordTranslations)
            {
                word.OriginalWord = wordSuite.WordTranslations.First(x => x.Id == word.Id).OriginalWord;
                if (word.TranslationWord == word.OriginalWord)
                {
                    word.Result = true;
                    _progressService.IncrementProgress(answer.Id, word.Id);
                }
            }           
        }

    }
}
