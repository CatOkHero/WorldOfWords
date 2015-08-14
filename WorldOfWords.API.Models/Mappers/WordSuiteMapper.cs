using System;
using System.Collections.Generic;
using System.Linq;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public class WordSuiteMapper:IWordSuiteMapper
    {
        public CourseWordSuiteModel Map(WordSuite wordSuite)
        {
            return new CourseWordSuiteModel()
            {
                Id = wordSuite.Id,
                Name = wordSuite.Name
            };
        }
        public WordSuite Map(CourseWordSuiteModel wordSuite)
        {
            return new WordSuite()
            {
                Id = wordSuite.Id,
                Name = wordSuite.Name,
            };
        }

        public List<CourseWordSuiteModel> Map(List<WordSuite> WordSuites)
        {
            return WordSuites.Select(x => Map(x)).ToList();
        }

        public WordSuite Map(WordSuiteModel wordSuite)
        {
            if (wordSuite == null)
            {
                throw new ArgumentNullException("wordSuite");
            }
            return new WordSuite()
            {
                Name = wordSuite.Name,
                LanguageId = wordSuite.LanguageId,
                OwnerId = wordSuite.OwnerId,
                PrototypeId = wordSuite.PrototypeId,
                Threshold = wordSuite.Threshold,
                QuizResponseTime = wordSuite.QuizResponseTime
            };  
        }

        public WordSuite Map(WordSuiteEditModel wordSuite)
        {
            if (wordSuite == null)
            {
                throw new ArgumentNullException("wordSuite");
            }
            return new WordSuite()
            {
                Id = wordSuite.Id,
                Name = wordSuite.Name,
                LanguageId = wordSuite.LanguageId,
                Threshold = wordSuite.Threshold,
                QuizResponseTime = wordSuite.QuizResponseTime
            };     
        }

        public WordSuiteEditModel MapForEdit(WordSuite wordSuite)
        {
            if (wordSuite == null)
            {
                throw new ArgumentNullException("wordSuite");
            }
            return new WordSuiteEditModel()
            {
                Id = wordSuite.Id,
                Name = wordSuite.Name,
                Language = wordSuite.Language.Name,
                LanguageId = wordSuite.LanguageId,
                Threshold = wordSuite.Threshold,
                QuizResponseTime = wordSuite.QuizResponseTime
            };
        }

        public List<WordSuiteEditModel> MapRangeForEdit (List<WordSuite> wordSuites)
        {
            if (wordSuites == null)
            {
                throw new ArgumentNullException("wordSuites");
            }
            return wordSuites.Select(ws => MapForEdit(ws)).ToList();
        }
    }
}
