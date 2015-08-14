using System.Collections.Generic;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services.Services
{
    public class WordService : IWordService
    {
        public List<Word> GetTopBySearchWord(string searchWord, int languageId, int count)
        {
            List<Word> words;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                words = context.Words
                    .Where(w => w.Value.Contains(searchWord) &&
                                w.LanguageId == languageId)
                    .OrderBy(w => w.Value.IndexOf(searchWord))
                    .ThenBy(w => w.Value)
                    .Take(count)
                    .ToList();
            }
            return words;
        }

        public List<Word> GetAllWords()
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Words.ToList();
            }
        }

        public List<Word> GetAllWordsBySpecificLanguage(int languageId)
        {

            using (var context = new WorldOfWordsDatabaseContext())
            {
                return (from words in context.Words where words.LanguageId == languageId select words).ToList();
            }

        }

        public int Exists(string value, int languageId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var word = context.Words.FirstOrDefault(w => w.Value == value && w.LanguageId == languageId);

                if (word != null)
                {
                    return word.Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Add(Word word)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                if (Exists(word.Value, word.LanguageId) > 0)
                {
                    return -1;
                }
                context.Words.Add(word);
                context.SaveChanges();
                return word.Id;
            }
        }


        public Word GetWordById(int id)
        {
            using(var context = new WorldOfWordsDatabaseContext())
            {
                return context.Words.Where(item => item.Id == id).FirstOrDefault();
            }
        }
    }
}