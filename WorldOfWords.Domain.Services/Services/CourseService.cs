using System.Collections.Generic;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;
using System.Data.Entity;
using System;

namespace WorldOfWords.Domain.Services
{
    public class CourseService : ICourseService
    {
        public List<Course> GetAllCourses(int userId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Courses.
                    Where(c => c.OwnerId == userId).
                    Include(course => course.WordSuites).
                    Include(course => course.Language).ToList();
            }
        }

        public List<Course> GetStudentCourses(int userId)
        {
            List<Course> courses;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                courses = (context.Users.
                    Include(user => user.Enrollments.Select(enrollment => enrollment.Group).Select(group => group.Course).Select(course => course.Language)).
                    Include(user => user.Enrollments.Select(enrollment => enrollment.Group).Select(group => group.Course).Select(course => course.WordSuites))
                    .First(x => x.Id == userId).Enrollments.Select(x => x.Group).Select(x => x.Course)).ToList();
            }
            foreach (var course in courses)
            {
                course.WordSuites = course.WordSuites.Where(x => (x.OwnerId == userId && x.PrototypeId != null)).ToList();
            }
            return courses;
        }

        public List<Course> GetTeacherCourses(int userId)
        {
            List<Course> courses;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                courses = context.Courses.
                    Include(course => course.WordSuites).
                    Include(x => x.Language).
                    Where(course => course.OwnerId == userId).ToList();
            }
            foreach (var course in courses)
            {
                course.WordSuites = course.WordSuites.Where(x => (x.OwnerId == userId && x.PrototypeId == null)).ToList();
            }
            return courses;
        }

        public Course GetById(int id)
        {
            Course course;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                course = context.Courses.
                    Include(x => x.WordSuites.Select(wp => wp.WordProgresses)).
                    Include(x => x.Language).
                    Include(x => x.Groups).
                    FirstOrDefault(x => x.Id == id);
            }
            if (course != null)
            {
                course.WordSuites = course.WordSuites.Where(x => (x.PrototypeId == null)).ToList();
            }
            return course;
        }

        public Course GetById(int id, int userId)
        {
            Course course;
            using (var context = new WorldOfWordsDatabaseContext())
            {
                course = context.Courses.
                    Include(x => x.WordSuites.Select(wp => wp.WordProgresses)).
                    Include(x => x.Language).
                    First(x => x.Id == id);
            }
            course.WordSuites = course.WordSuites.Where(x => (x.OwnerId == userId && x.PrototypeId != null)).ToList();
            return course;
        }

        public double GetProgress(int id, int userId)
        {
            var course = GetById(id, userId);
            double progress = 0;
            foreach (var suite in course.WordSuites)
            {
                double allProgress = suite.Threshold * suite.WordProgresses.Count;
                var userProgress = (int)suite.WordProgresses.Select(x => x.Progress).Sum();
                progress += userProgress / allProgress;
            }
            return Math.Round((progress / course.WordSuites.Count) * 100, 2);
        }

        public int Add(Course course, List<int> wordSuitesId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                context.Courses.Add(course);
                context.SaveChanges();
                AddWordSuitesToCourse(course.Id, wordSuitesId);
                context.SaveChanges();
                return course.Id;
            }
        }

        public void Delete(int courseId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                context.Courses.Remove(context.Courses.First(x => x.Id == courseId));
                context.SaveChanges();
            }
        }

        public void EditCourse(Course courseModel, List<int> wordSuitesId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var course = context.Courses.First(x => x.Id == courseModel.Id);
                course.Name = courseModel.Name;
                var wordSuites = course.WordSuites.
                    Where(ws => (!wordSuitesId.Contains(ws.Id) && ws.PrototypeId == null)).ToList();
                RemoveWordSuitesFromCourse(course.Id, wordSuites);
                wordSuitesId.RemoveAll(x => course.WordSuites.Select(ws => ws.Id).Contains(x));
                AddWordSuitesToCourse(course.Id, wordSuitesId);
                context.SaveChanges();
            }

        }

        private void AddWordSuitesToCourse(int courseId, List<int> wordSuitesId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var course = context.Courses.First(x => x.Id == courseId);
                foreach (var wordSuiteId in wordSuitesId)
                {
                    course.WordSuites.Add(context.WordSuites.FirstOrDefault(x => x.Id == wordSuiteId));
                }
                context.SaveChanges();
            }
        }

        private void RemoveWordSuitesFromCourse(int courseId, List<WordSuite> wordSuites)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var course = context.Courses.First(x => x.Id == courseId);
                foreach (var wordSuite in wordSuites)
                {
                    course.WordSuites.Remove(wordSuite);
                }
                context.SaveChanges();
            }
        }
    }
}
