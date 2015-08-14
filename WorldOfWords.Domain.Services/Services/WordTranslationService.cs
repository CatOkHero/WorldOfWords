using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services
{
    public class WordTranslationService : IWordTranslationService
    {
        private const string TRANSLATION_LANGUAGE = "Ukrainian";

        public List<WordTranslation> GetTopBySearchWord(string searchWord, int languageId, int count)
        {
            List<WordTranslation> wordTranslations;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                wordTranslations = context.WordTranslations
                    .Where(w => w.OriginalWord.LanguageId == languageId &&
                          (w.OriginalWord.Value.Contains(searchWord) || w.TranslationWord.Value.Contains(searchWord)))
                    .OrderBy(w => w.OriginalWord.Value.IndexOf(searchWord))
                    .ThenBy(w => w.OriginalWord.Value)
                    .Take(count)
                    .Include(w => w.OriginalWord)
                    .Include(w => w.TranslationWord)
                    .ToList();
            }
            return wordTranslations;
        }

        public List<WordTranslation> GetByWordSuiteID(int id)
        {
            List<WordTranslation> wordTranslations;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                wordTranslations = context.WordTranslations
                    .Where(wt => context.WordProgresses
                        .Where(wp => wp.WordSuiteId == id)
                        .Select(wp => wp.WordTranslationId)
                        .Contains(wt.Id))
                    .Include(wt => wt.OriginalWord)
                    .Include(wt => wt.TranslationWord)
                    .ToList();
            }
            return wordTranslations;
        }

        public int Exists(int originalWordId, int translationWordId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var wordTranslation = context.WordTranslations
                    .FirstOrDefault(wt => wt.OriginalWordId == originalWordId &&
                                          wt.TranslationWordId == translationWordId);

                if (wordTranslation != null)
                {
                    return wordTranslation.Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Add(WordTranslation wordTranslation)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                context.WordTranslations.Add(wordTranslation);
                context.SaveChanges();
                return wordTranslation.Id;
            }
        }
        
        public List<WordTranslation> GetWordsFromInterval(int startOfInterval, int endOfInterval, int languageId)
        {
            List<WordTranslation> wordTranslations;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                if (startOfInterval >= context.WordTranslations.Count() || startOfInterval < 0
                    || startOfInterval > endOfInterval || endOfInterval > context.WordTranslations.Count())
                    throw new System.ArgumentException("Start of interval is bigger than end");
                wordTranslations = context.WordTranslations
                    .Where(item => item.OriginalWord.LanguageId == languageId)
                    .OrderBy(item => item.OriginalWord.Value)
                    .Skip(startOfInterval)
                    .Take(endOfInterval - startOfInterval)
                    .Include(w => w.OriginalWord)
                    .Include(w => w.TranslationWord)
                    .ToList();
            }
            return wordTranslations;
        }

        public int GetAmountOfWordTranslationsByLanguage(int langID)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
               return context.WordTranslations.Where(item => item.OriginalWord.LanguageId == langID).Count();
            }
        }

        public WordTranslation GetWordTranslationById(int id)
        {
            using(var context = new WorldOfWordsDatabaseContext())
            {
                return context.WordTranslations.Where(item => item.Id == id).FirstOrDefault();
            }
        }
    
        public List<WordTranslation> GetWordsWithSearchValue(string searchValue, int startOfInterval, int endOfInterval, int languageId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                if (startOfInterval >= context.WordTranslations.Count() || startOfInterval < 0
                    || startOfInterval > endOfInterval || endOfInterval > context.WordTranslations.Count())
                    throw new System.ArgumentException("Start of interval is bigger than end");
                return context.WordTranslations
                    .Where(w => w.OriginalWord.LanguageId == languageId &&
                          (w.OriginalWord.Value.Contains(searchValue) || w.TranslationWord.Value.Contains(searchValue)))
                    .OrderBy(w => w.OriginalWord.Value)
                    .Skip(startOfInterval)
                    .Take(endOfInterval - startOfInterval)
                    .Include(w => w.OriginalWord)
                    .Include(w => w.TranslationWord)
                    .ToList();
            }
        }

        public int GetAmountOfWordsBySearchValues(string searchValue, int languageId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.WordTranslations
                    .Where(w => w.OriginalWord.LanguageId == languageId &&
                          (w.OriginalWord.Value.Contains(searchValue) || w.TranslationWord.Value.Contains(searchValue)))
                    .Count();
            }
        }
    }
}
