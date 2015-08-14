using System;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;
using WorldOfWords.Infrastructure.Data.EF.Contracts;

namespace WorldOfWords.Domain.Services
{
    public class WorldOfWordsUow : IDisposable, IWorldOfWordsUow
    {
        private WorldOfWordsDatabaseContext context = new WorldOfWordsDatabaseContext();
        private bool disposed = false;

        private IRepository<Role> roleRepository;
        private IRepository<Word> wordRepostiory;
        private IRepository<Language> languageRepository;
        private IRepository<WordProgress> wordProgressRepository;
        private IRepository<WordTranslation> wordTranslationRepository;
        private IRepository<Group> groupRepository;
        private IRepository<Enrollment> enrollmentRepository;
        private IWordSuiteRepository wordSuiteRepository;
        private IUserRepository userRepository;
        private ICourseRepository courseRepository;


        public IRepository<Enrollment> EnrollmentRepository
        {
            get
            {
                if (enrollmentRepository == null)
                {
                    enrollmentRepository = new EFRepository<Enrollment>(context);
                }
                return enrollmentRepository;
            }
        }
        public IRepository<WordProgress> WordProgressRepository
        {
            get
            {
                if (wordProgressRepository == null)
                {
                    wordProgressRepository = new EFRepository<WordProgress>(context);
                }
                return wordProgressRepository;
            }
        }
        public IRepository<WordTranslation> WordTranslationRepository
        {
            get
            {
                if (wordTranslationRepository == null)
                {
                    wordTranslationRepository = new EFRepository<WordTranslation>(context);
                }
                return wordTranslationRepository;
            }

        }
        public IRepository<Group> GroupRepository
        {
            get
            {
                if (groupRepository == null)
                {
                    groupRepository = new EFRepository<Group>(context);
                }
                return groupRepository;
            }

        }
        public IRepository<Role> RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new EFRepository<Role>(context);
                }
                return roleRepository;
            }
        }
        public IRepository<Word> WordRepository
        {
            get
            {
                if (wordRepostiory == null)
                {
                    wordRepostiory = new EFRepository<Word>(context);
                }
                return wordRepostiory;
            }
        }
        public IWordSuiteRepository WordSuiteRepository
        {
            get
            {
                if (wordSuiteRepository == null)
                {
                    wordSuiteRepository = new WordSuiteRepository(context);
                }
                return wordSuiteRepository;
            }
        }
        public IRepository<Language> LanguageRepository
        {
            get
            {
                if (languageRepository == null)
                {
                    languageRepository = new EFRepository<Language>(context);
                }
                return languageRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }
        public ICourseRepository CourseRepository
        {
            get
            {
                if (courseRepository == null)
                {
                    courseRepository = new CourseRepository(context);
                }
                return courseRepository;
            }
        }
        
        public void Save()
        {
            context.SaveChanges();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
