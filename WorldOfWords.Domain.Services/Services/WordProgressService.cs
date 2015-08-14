using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services
{
    public class WordProgressService : IWordProgressService
    {
        public bool IsStudentWord(WordProgress wordProgress)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context
                        .WordProgresses
                        .Single(wp => wp.WordSuiteId == wordProgress.WordSuiteId &&
                                      wp.WordTranslationId == wordProgress.WordTranslationId).IsStudentWord;
            }
        }

        public bool CopyProgressesForUsersInGroup(List<User> users, int groupId)
        {
            try
            {
                using(var context = new WorldOfWordsDatabaseContext())
                {
                    var group = context.Groups.FirstOrDefault(g => g.Id == groupId);
                    if(group == null)
                    {
                        return false;
                    }
                    var originalWordsuites = group.Course.WordSuites.Where(w => w.PrototypeId == null);
                    var progressesToAdd = new List<WordProgress>();
                    foreach(var user in users)
                    {
                        foreach(var wordsuite in originalWordsuites)
                        {
                            var copiedWordSuite = group.Course.WordSuites.FirstOrDefault(w => w.PrototypeId == wordsuite.Id && w.OwnerId == user.Id);
                            progressesToAdd.AddRange(copiedWordSuite.PrototypeWordSuite.WordProgresses.Select(wp => new WordProgress
                                {
                                    WordSuiteId = copiedWordSuite.Id,
                                    WordTranslationId = wp.WordTranslationId,
                                    Progress = 0
                                }));
                        }
                    }
                    context.WordProgresses.AddRange(progressesToAdd);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddRange(List<WordProgress> wordProgressRange)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var wordProgresses = context.WordProgresses;
                foreach (var wordProgress in wordProgressRange)
                {
                    wordProgresses.AddOrUpdate(wordProgress);
                }
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveRange(List<WordProgress> wordProgressRange)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                wordProgressRange.ForEach(wp =>
                    context
                    .WordProgresses
                    .Remove(context
                            .WordProgresses
                            .Single(dbWp => dbWp.WordSuiteId == wp.WordSuiteId
                                            && dbWp.WordTranslationId == wp.WordTranslationId)));
                context.SaveChanges();
            }
            return true;
        }

        public bool RemoveByStudent(WordProgress wordProgress)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                if (IsStudentWord(wordProgress))
                {
                    context
                    .WordProgresses
                    .Remove(context
                        .WordProgresses
                        .Single(wp => wp.WordSuiteId == wordProgress.WordSuiteId &&
                                      wp.WordTranslationId == wordProgress.WordTranslationId));
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool IncrementProgress(int wordSuiteId, int wordTranslationId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var wordProgress = context.WordProgresses.First(x => (x.WordSuiteId == wordSuiteId
                            && x.WordTranslationId == wordTranslationId));
                ++(wordProgress.Progress);
                context.WordProgresses.AddOrUpdate(wordProgress);
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveProgressesForEnrollment(int enrollmentId)
        {
            using(var context = new WorldOfWordsDatabaseContext())
            {
                try
                {
                    Enrollment enrollment = context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId);
                    if (enrollment == null)
                    {
                        return false;
                    }
                    List<int> originalWordsuitesIds = enrollment.Group.Course.WordSuites.Where(w => w.PrototypeId == null).Select(w => w.Id).ToList();
                    var wordsuitesToDeleteFrom = enrollment.Group.Course.WordSuites
                        .Where(w => w.OwnerId == enrollment.User.Id && w.PrototypeId != null 
                            && originalWordsuitesIds.Contains((int)w.PrototypeId)).ToList();
                    context.WordProgresses.RemoveRange(wordsuitesToDeleteFrom.SelectMany(ws => ws.WordProgresses));
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
