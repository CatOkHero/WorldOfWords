using System.Collections.Generic;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Domain.Services.IServices;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services.Services
{
    /// <summary>
    /// Responsible for obtaining and manipulating the list of languages.
    /// </summary>
    public class LanguageService : ILanguageService
    {
        /// <summary>
        /// Adds a new language to the database.
        /// </summary>
        /// <param name="language">The language to be added to the database.</param>
        /// <returns>The id of a new database record, or -1, if such language already exists.</returns>
        public int AddLanguage(Language language)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                if (context
                    .Languages
                    .Any(l => l.Name == language.Name))
                {
                    return -1;
                }
                context.Languages.Add(language);
                context.SaveChanges();
                return language.Id;
            }
        }

        /// <summary>
        /// Removes language from the database.
        /// </summary>
        /// <param name="id"> ID of the language to be removed from the database.</param>
        /// <returns>True, if language successfully deleted, false, if no language with such ID</returns>
        public bool RemoveLanguage(int id)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var language = context.Languages.SingleOrDefault(l => l.Id == id);

                if (language != null)
                {
                    if (language.Courses.Count == 0) 
                    {
                        if (language.WordSuites.Count != 0)
                        {
                            var wordSuites = context.WordSuites.Where(ws => ws.LanguageId == id);
                            var wordProgresses = context.WordProgresses.Where(wp => wordSuites.Select(ws => ws.Id).Contains(wp.WordSuiteId));
                            context.WordProgresses.RemoveRange(wordProgresses);
                            context.WordSuites.RemoveRange(wordSuites);
                        }
                        if (language.Words.Count != 0)
                        {
                            var wordTranslations = context.WordTranslations.Where(wt => wt.OriginalWord.LanguageId == id);
                            context.WordTranslations.RemoveRange(wordTranslations);

                            var words = context.Words.Where(w => w.LanguageId == id);
                            context.Words.RemoveRange(words);
                        }

                        context.Languages.Remove(language);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns a list of all languages in the database.
        /// </summary>
        /// <returns></returns>
        public List<Language> GetAllLanguages()
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Languages.ToList();
            }
        }
    }
}