using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;
using System.Data.Entity;
using System;

namespace WorldOfWords.Domain.Services
{
    public class WordSuiteService : IWordSuiteService
    {
        public WordSuite GetByID(int id)
        {
            WordSuite wordSuite;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                wordSuite = context
                            .WordSuites
                            .Include(ws => ws.Language)
                            .Include(ws => ws.WordProgresses.Select(wp => wp.WordTranslation).Select(wt => wt.OriginalWord))
                            .Include(ws => ws.WordProgresses.Select(wp => wp.WordTranslation).Select(wt => wt.TranslationWord))
                            .First(x => x.Id == id);
            }
            return wordSuite;
        }

        public void SetTime(int id)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var suite = context
                    .WordSuites
                    .First(ws => ws.Id == id);
                suite.QuizStartTime = DateTime.Now;
                context.SaveChanges();
            }
        }

        public bool RemoveWordSuitesForEnrollment(int enrollmentId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                try
                {
                    Enrollment enrollment = context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId);
                    if (enrollment == null)
                    {
                        return false;
                    }
                    List<int> originalWordsuitesIds = enrollment.Group.Course.WordSuites.Where(w => w.PrototypeId == null).Select(w => w.Id).ToList();
                    context.WordSuites.RemoveRange(enrollment.Group.Course.WordSuites
                        .Where(w => w.OwnerId == enrollment.User.Id && w.PrototypeId != null
                            && originalWordsuitesIds.Contains((int)w.PrototypeId)));
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public WordSuite GetWithNotStudiedWords(int id)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var suite = context.WordSuites
                    .Include(ws => ws.WordProgresses.Select(wp => wp.WordTranslation).Select(wt => wt.OriginalWord))
                    .Include(ws => ws.WordProgresses.Select(wp => wp.WordTranslation).Select(wt => wt.TranslationWord))
                    .First(ws => ws.Id == id);
                if (suite != null)
                {
                    var n = suite.Threshold;
                    suite.WordProgresses = suite.WordProgresses.Where(x => (x.Progress < n)).ToList();
                }
                return suite;
            }
        }

        public double GetWordSuiteProgress(int id)
        {
            double progress = 0;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var suite = context
                    .WordSuites
                    .Include(ws => ws.WordProgresses)
                    .First(ws => ws.Id == id);
                if (suite != null)
                {
                    double allProgress = suite.Threshold*suite.WordProgresses.Count;
                    var userProgress = (int) suite.WordProgresses.Select(x => x.Progress).Sum();
                    progress = userProgress/allProgress;
                }
                return Math.Round(progress*100, 2);
            }
        }

        public bool CopyWordsuitesForUsersByGroup(List<User> users, int groupId)
        {
            try
            {
                using (var context = new WorldOfWordsDatabaseContext())
                {
                    Group group = context.Groups.FirstOrDefault(g => g.Id == groupId);
                    if (group == null)
                    {
                        return false;
                    }
                    List<WordSuite> wordsuitesToCopy = group.Course.WordSuites.Where(w => w.PrototypeId == null).ToList();
                    List<WordSuite> wordsuitesToAdd = new List<WordSuite>();
                    foreach (var user in users)
                    {
                        wordsuitesToAdd.AddRange(wordsuitesToCopy.Select(w => new WordSuite
                            {
                                Name = w.Name,
                                LanguageId = w.LanguageId,
                                Threshold = w.Threshold,
                                QuizResponseTime = w.QuizResponseTime,
                                QuizStartTime = null,
                                OwnerId = user.Id,
                                PrototypeId = w.Id,
                                Courses = new[] { group.Course }
                            }));
                    }
                    context.WordSuites.AddRange(wordsuitesToAdd);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<WordSuite> GetTeacherWordSuites(int id)
        {
            List<WordSuite> wordSuites;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                wordSuites = context.WordSuites
                    .Include(ws => ws.Language)
                    .Where(ws => ws.OwnerId == id && ws.PrototypeId == null)
                    .ToList();
            }
            return wordSuites;
        }

        public List<WordSuite> GetWordSuitesByOwnerAndLanguageId(int userId, int languageId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
               return  context.WordSuites.
                    Where(ws => ws.OwnerId == userId && ws.PrototypeId == null && ws.LanguageId == languageId).
                    ToList();
            }
        }

        public int Add(WordSuite wordSuite)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                context.WordSuites.AddOrUpdate(wordSuite);
                context.SaveChanges();
                return wordSuite.Id;
            }
        }

        public bool Update(WordSuite wordSuite)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var oldWordSuite = context.WordSuites.First(ws => ws.Id == wordSuite.Id);
                oldWordSuite.Name = wordSuite.Name;
                oldWordSuite.Threshold = wordSuite.Threshold;
                oldWordSuite.QuizResponseTime = wordSuite.QuizResponseTime;

                context.WordSuites.AddOrUpdate(oldWordSuite);
                context.SaveChanges();
            }
            return true;
        }

        public bool Delete(int id)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var wordProgresses = context.WordProgresses;
                wordProgresses.RemoveRange(wordProgresses.Where(wp => wp.WordSuiteId == id));
                var wordSuites = context.WordSuites;
                wordSuites.Remove(wordSuites.FirstOrDefault(ws => ws.Id == id));
                context.SaveChanges();
                return true;
            }
        }
    }
}
