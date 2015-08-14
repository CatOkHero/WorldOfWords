using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using WorldOfWords.Domain.Models;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using WorldOfWords.Validation.Classes;

namespace WorldOfWords.Infrastructure.Data.EF.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WorldOfWordsDatabaseContext>
    {
        readonly PasswordHasher _passwordHasher = new PasswordHasher();
        readonly TokenValidation tokenValidation = new TokenValidation();

        #region UsersDataField
        private readonly User[] _userModels = new[]
        {
            new User()
            {
                Id = 1,
                Email = "admin@example.com",
                Name = "Admin",
                Password = "admin"
            },
            new User()
            {
                Id = 2,
                Email = "andriy@example.com",
                Name = "Andriy",
                Password = "4315"
            },
            new User()
            {
                Id = 3,
                Email = "nazar@example.com",
                Name = "Nazar",
                Password = "1111"
            },
            new User()
            {
                Id = 4,
                Email = "yura@example.com",
                Name = "Yura",
                Password = "5222"
            },
            new User()
            {
                Id = 5,
                Email = "yaryna@example.com",
                Name = "Yaruna",
                Password = "9021"
            },
            new User()
            {
                Id = 6,
                Email = "sasha@example.com",
                Name = "Sasha",
                Password = "1094"
            },
            new User()
            {
                Id = 7,
                Email = "slava@example.com",
                Name = "Slava",
                Password = "1842"
            }
        };
        #endregion

        #region WordSuitDataField
        private readonly WordSuite[] _wordsuitsModels = new[]
        {
            new WordSuite()
            {
                Id = 1,
                Name = "Days of the week",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 2,
                Name = "Seasons and months",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 3,
                Name = "Family members",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 4,
                Name = "Die Tage der Woche",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 5,
                Name = "Die Jahreszeiten und die Monate",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 6,
                Name = "Die Familienmitglieder",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 9,
                Name = "Les jours de la semaine",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 7,
                Name = "Saisons et mois",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            },
            new WordSuite()
            {
                Id = 8,
                Name = "Membres de la famille",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = null
            }
        };
        private readonly WordSuite[] _derivedWordSuitesModel = new[]
        {            
            //WordSuites for User with Id = 1
            new WordSuite()
            {
                Name = "Days of the week",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = 1
            },
            new WordSuite()
            {
                Name = "Seasons and months",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = 2
            },
            new WordSuite()
            {
                Name = "Family members",
                LanguageId = 1,
                OwnerId = 1,
                Threshold = 1,
                QuizResponseTime = 10,
                PrototypeId = 3
            },
            new WordSuite()
            {
                Name = "Die Tage der Woche",
                LanguageId = 2,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 4
            },
            new WordSuite()
            {
                Name = "Die Jahreszeiten und die Monate",
                LanguageId = 2,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 5
            },
            new WordSuite()
            {
                Name = "Die Familienmitglieder",
                LanguageId = 2,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 6
            },
            new WordSuite()
            {
                Name = "Les jours de la semaine",
                LanguageId = 3,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 9
            },
            new WordSuite()
            {
                Name = "Saisons et mois",
                LanguageId = 3,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 7
            },
            new WordSuite()
            {
                Name = "Membres de la famille",
                LanguageId = 3,
                OwnerId = 1,
                Threshold = 3,
                QuizResponseTime = 10,
                PrototypeId = 8
            },
            //Wordsuites for user with Id = 2
            new WordSuite()
            {
                Name = "Days of the week",
                LanguageId = 1,
                OwnerId = 2,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 1
            },
            new WordSuite()
            {
                Name = "Seasons and months",
                LanguageId = 1,
                OwnerId = 2,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 2
            },
            new WordSuite()
            {
                Name = "Family members",
                LanguageId = 1,
                OwnerId = 2,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 3
            },
            new WordSuite()
            {
                Name = "Die Tage der Woche",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 4
            },
            new WordSuite()
            {
                Name = "Die Jahreszeiten und die Monate",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 5
            },
            new WordSuite()
            {
                Name = "Die Familienmitglieder",
                LanguageId = 2,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 6
            },
            new WordSuite()
            {
                Name = "Les jours de la semaine",
                LanguageId = 3,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 9
            },
            new WordSuite()
            {
                Name = "Saisons et mois",
                LanguageId = 3,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 7
            },
            new WordSuite()
            {
                Name = "Membres de la famille",
                LanguageId = 3,
                OwnerId = 2,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 8
            },
            //Wordsuites for user with Id = 3
            new WordSuite()
            {
                Name = "Days of the week",
                LanguageId = 1,
                OwnerId = 3,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 1
            },
            new WordSuite()
            {
                Name = "Seasons and months",
                LanguageId = 1,
                OwnerId = 3,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 2
            },
            new WordSuite()
            {
                Name = "Family members",
                LanguageId = 1,
                OwnerId = 3,
                Threshold = 1,
                QuizResponseTime = 10,
		        PrototypeId = 3
            },
            new WordSuite()
            {
                Name = "Die Tage der Woche",
                LanguageId = 2,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 4
            },
            new WordSuite()
            {
                Name = "Die Jahreszeiten und die Monate",
                LanguageId = 2,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 5
            },
            new WordSuite()
            {
                Name = "Die Familienmitglieder",
                LanguageId = 2,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 6
            },
            new WordSuite()
            {
                Name = "Les jours de la semaine",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 9
            },
            new WordSuite()
            {
                Name = "Saisons et mois",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 7
            },
            new WordSuite()
            {
                Name = "Membres de la famille",
                LanguageId = 3,
                OwnerId = 3,
                Threshold = 3,
                QuizResponseTime = 10,
		        PrototypeId = 8
            }
        };
        #endregion

        #region CoursesDataField
        private readonly Course[] _courseModels = new[]
        {
            new Course()
            {
                Id = 1,
                Name = "English.A1",
                IsPrivate = true,
                LanguageId = 1,
                OwnerId = 1,
                WordSuites = new List<WordSuite>()
            },
            new Course()
            {
                Id = 2,
                Name = "German.A1",
                IsPrivate = true,
                LanguageId = 2,
                OwnerId = 2,
                WordSuites = new List<WordSuite>()
            },
            new Course()
            {
                Id = 3,
                Name = "French.A1",
                IsPrivate = true,
                LanguageId = 3,
                OwnerId = 3,
                WordSuites = new List<WordSuite>()
            }
        };


        #endregion

        #region LanguageDataField
        private readonly Language[] _languageModels = new[]
        {
            new Language()
            {
                Id = 1,
                Name = "English"
            },
            new Language()
            {
                Id = 2,
                Name = "German"
            },
            new Language()
            {
                Id = 3,
                Name = "French"
            },
            new Language()
            {
                Id = 4,
                Name = "Ukrainian"
            }
        };
        #endregion

        #region FamilyWordsDataField
        private readonly Word[] _familyWordModels = new[]
        {
            new Word()
            {
                Id = 1,
                Value = "mother",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 2,
                Value = "father",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 3,
                Value = "sister",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 4,
                Value = "brother",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 5,
                Value = "son",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 6,
                Value = "daughter",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 7,
                Value = "nephew",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 8,
                Value = "niece",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 9,
                Value = "grandmother",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 10,
                Value = "grandfather",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 11,
                Value = "uncle",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 12,
                Value = "aunt",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 13,
                Value = "мама",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 14,
                Value = "тато",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 15,
                Value = "сестра",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 16,
                Value = "брат",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 17,
                Value = "син",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 18,
                Value = "дочка",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 19,
                Value = "племінник",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 20,
                Value = "племінниця",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 21,
                Value = "бабуся",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 22,
                Value = "дідусь",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 23,
                Value = "дядько",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 24,
                Value = "тітка",
                LanguageId = 4,
            },
             
        };
        #endregion

        #region DaysWordsDataField
        private readonly Word[] _dayWordModels = new[]
        {
            new Word()
            {
                Id = 25,
                Value = "Sunday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 26,
                Value = "Monday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 27,
                Value = "Tuesday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 28,
                Value = "Wednesday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 29,
                Value = "Thursday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 30,
                Value = "Friday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 31,
                Value = "Saturday",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 32,
                Value = "неділя",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 33,
                Value = "понеділок",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 34,
                Value = "вівторок",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 35,
                Value = "середа",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 36,
                Value = "четвер",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 37,
                Value = "п'ятниця",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 38,
                Value = "субота",
                LanguageId = 4,
            },
        };
        #endregion

        #region MonthWordsDataField
        private readonly Word[] _monthsWordModels = new[]
        {
            new Word()
            {
                Id = 39,
                Value = "January",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 40,
                Value = "February",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 41,
                Value = "March",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 42,
                Value = "April",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 43,
                Value = "May",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 44,
                Value = "June",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 45,
                Value = "July",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 46,
                Value = "August",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 47,
                Value = "September",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 48,
                Value = "October",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 49,
                Value = "November",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 50,
                Value = "December",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 51,
                Value = "winter",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 52,
                Value = "spring",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 53,
                Value = "summer",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 54,
                Value = "autumn",
                LanguageId = 1,
            },
            new Word()
            {
                Id = 55,
                Value = "січень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 56,
                Value = "лютий",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 57,
                Value = "березень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 58,
                Value = "квітень",
                LanguageId = 4,
            },

            new Word()
            {
                Id = 59,
                Value = "травень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 60,
                Value = "червень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 61,
                Value = "липень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 62,
                Value = "серпень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 63,
                Value = "вересень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 64,
                Value = "жовтень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 65,
                Value = "листопад",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 66,
                Value = "грудень",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 67,
                Value = "зима",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 68,
                Value = "весна",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 69,
                Value = "літо",
                LanguageId = 4,
            },
            new Word()
            {
                Id = 70,
                Value = "осінь",
                LanguageId = 4,
            }             
        };
        #endregion

        #region WordTranslationDataField
        private readonly WordTranslation[] _wordTranslationModels = new[]
        {
            new WordTranslation()
            {
                Id = 1,
                OriginalWordId = 1,
                TranslationWordId = 13,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 2,
                OriginalWordId = 2,
                TranslationWordId = 14,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 3,
                OriginalWordId = 3,
                TranslationWordId = 15,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 4,
                OriginalWordId = 4,
                TranslationWordId = 16,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 5,
                OriginalWordId = 5,
                TranslationWordId = 17,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 6,
                OriginalWordId = 6,
                TranslationWordId = 18,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 7,
                OriginalWordId = 7,
                TranslationWordId = 19,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 8,
                OriginalWordId = 8,
                TranslationWordId = 20,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 9,
                OriginalWordId = 9,
                TranslationWordId = 21,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 10,
                OriginalWordId = 10,
                TranslationWordId = 22,
                OwnerId = 1,
            }, 
            new WordTranslation()
            {
                Id = 11,
                OriginalWordId = 11,
                TranslationWordId = 23,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 12,
                OriginalWordId = 12,
                TranslationWordId = 24,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 13,
                OriginalWordId = 25,
                TranslationWordId = 32,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 14,
                OriginalWordId = 26,
                TranslationWordId = 33,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 15,
                OriginalWordId = 27,
                TranslationWordId = 34,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 16,
                OriginalWordId = 28,
                TranslationWordId = 35,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 17,
                OriginalWordId = 29,
                TranslationWordId = 36,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 18,
                OriginalWordId = 30,
                TranslationWordId = 37,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 19,
                OriginalWordId = 31,
                TranslationWordId = 38,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 20,
                OriginalWordId = 39,
                TranslationWordId = 55,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 21,
                OriginalWordId = 40,
                TranslationWordId = 56,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 22,
                OriginalWordId = 41,
                TranslationWordId = 57,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 23,
                OriginalWordId = 42,
                TranslationWordId = 58,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 24,
                OriginalWordId = 43,
                TranslationWordId = 59,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 25,
                OriginalWordId = 44,
                TranslationWordId = 60,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 26,
                OriginalWordId = 45,
                TranslationWordId = 61,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 27,
                OriginalWordId = 46,
                TranslationWordId = 62,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 28,
                OriginalWordId = 47,
                TranslationWordId = 63,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 29,
                OriginalWordId = 48,
                TranslationWordId = 64,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 30,
                OriginalWordId = 49,
                TranslationWordId = 65,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 31,
                OriginalWordId = 50,
                TranslationWordId = 66,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 32,
                OriginalWordId = 51,
                TranslationWordId = 67,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 33,
                OriginalWordId = 52,
                TranslationWordId = 68,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 34,
                OriginalWordId = 53,
                TranslationWordId = 69,
                OwnerId = 1,
            },
            new WordTranslation()
            {
                Id = 35,
                OriginalWordId = 54,
                TranslationWordId = 70,
                OwnerId = 1,
            }
        };
        #endregion

        #region RolesDataField
        private readonly Role[] _roleModels = new[]
        {
            new Role()
            {
                Id=1,
                Name="Admin"
            },
            new Role()
            {
                Id=2,
                Name="Teacher"
            },
            new Role()
            {
                Id=3,
                Name="Student"
            }
        };
        #endregion

        #region GroupsDataField
        private readonly Group[] _groupsModels = new[]
        {
            new Group{
                Id=1,
                CourseId=1,
                OwnerId=1,
                Name="Basic English"
            },
            new Group{
                Id=2,
                CourseId=2,
                OwnerId=1,
                Name="Basic German"
            },
            new Group{
                Id=3,
                CourseId=3,
                OwnerId=1,
                Name="Basic French"
            }
        };
        #endregion

        #region EnrollmentsDataField
        private readonly Enrollment[] _enrollmentsModels = new[]
        {
            new Enrollment{
                Id=1,
                Date=DateTime.Now,
                GroupId=1,
                UserId=1
            },
            new Enrollment{
                Id=2,
                Date=DateTime.Now,
                GroupId=1,
                UserId=2
            },
            new Enrollment{
                Id=3,
                Date=DateTime.Now,
                GroupId=1,
                UserId=3
            },
            new Enrollment{
                Id=4,
                Date=DateTime.Now,
                GroupId=2,
                UserId=1
            },
            new Enrollment{
                Id=5,
                Date=DateTime.Now,
                GroupId=2,
                UserId=2
            },
            new Enrollment{
                Id=6,
                Date=DateTime.Now,
                GroupId=2,
                UserId=3
            },
            new Enrollment{
                Id=7,
                Date=DateTime.Now,
                GroupId=3,
                UserId=1
            },
            new Enrollment{
                Id=8,
                Date=DateTime.Now,
                GroupId=3,
                UserId=2
            },
            new Enrollment{
                Id=9,
                Date=DateTime.Now,
                GroupId=3,
                UserId=3
            }
        };
        #endregion

        #region WordProgresseField
        private readonly int[] _idOfWordSuitesToCopyWordProgresses = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly int[] _idOfUsersToAddWordProgresses = new[] { 1, 2, 3 };
        #endregion

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        //private void CopyWordsFromWordSuiteForUser(WorldOfWordsDatabaseContext context, int userId, int prototypeId)
        //{
        //    var wordSuiteToFill = context.WordSuites.FirstOrDefault(ws => ws.OwnerId == userId && ws.PrototypeId == prototypeId);
        //    var prototypeWordsuite = context.WordSuites.FirstOrDefault(ws => ws.Id == prototypeId && ws.PrototypeId == null);
        //    var wordProgressesToCopy = prototypeWordsuite.WordProgresses;
        //    context.WordProgresses.AddOrUpdate(wp => new { wp.WordSuiteId, wp.WordTranslationId },
        //        wordProgressesToCopy.Select(wp => new WordProgress
        //            {
        //                WordSuiteId = wordSuiteToFill.Id,
        //                WordTranslationId = wp.WordTranslationId,
        //                Progress = 0
        //            }).ToArray());
        //    context.SaveChanges();
        //}

        public void HashUserPasswords(User[] users)
        {
            foreach (var user in users)
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
                //user.HashedToken = Sha256Hash(tokenValidation.RandomString(20));
            }
        }

        protected override void Seed(WorldOfWordsDatabaseContext context)
        {
            base.Seed(context);

            //context
            //    .IncomingUsers
            //    .ToList()
            //    .ForEach(user => context.IncomingUsers.Remove(user));
            //context.SaveChanges();
            //context.Languages.AddOrUpdate(t => t.Id, _languageModels);
            //context.SaveChanges();
            HashUserPasswords(_userModels);
            context.Users.AddOrUpdate(_userModels[0]);
            context.SaveChanges();
            context.Roles.AddOrUpdate(t => t.Id, _roleModels);
            context.SaveChanges();
            //SeedHelper.AddOrUpdate(context, t => new
            //    {
            //        Name = t.Name,
            //        OwnerId = t.OwnerId,
            //        PrototypeId = t.PrototypeId
            //    }, _wordsuitsModels);
            //context.SaveChanges();
            //SeedHelper.AddOrUpdate(context, t => new
            //{
            //    Name = t.Name,
            //    OwnerId = t.OwnerId,
            //    PrototypeId = t.PrototypeId
            //}, _derivedWordSuitesModel);
            //context.SaveChanges();
            //context.Courses.AddOrUpdate(t => t.Id, _courseModels);
            //context.SaveChanges();
            //context.Groups.AddOrUpdate(t => t.Id, _groupsModels);
            //context.SaveChanges();
            //context.Enrollments.AddOrUpdate(t => new { GroupId = t.GroupId, UserId = t.UserId }, _enrollmentsModels);
            //context.SaveChanges();
            //context.Words.AddOrUpdate(t => t.Id, _familyWordModels);
            //context.SaveChanges();
            //context.Words.AddOrUpdate(t => t.Id, _dayWordModels);
            //context.SaveChanges();
            //context.Words.AddOrUpdate(t => t.Id, _monthsWordModels);
            //context.SaveChanges();
            //context.WordTranslations.AddOrUpdate(t => t.Id, _wordTranslationModels);
            //context.SaveChanges();

            ////Distribute wordsuites
            //var i = 0;
            //var wordSuites = context.WordSuites.Where(ws => _idOfWordSuitesToCopyWordProgresses.Contains(ws.Id)).ToList();
            //var coursesToFix = context.Courses.Where(c => c.Id == 1 || c.Id == 2 || c.Id == 3);
            //foreach (var course in coursesToFix)
            //{
            //    if (course.WordSuites.Count == 0)
            //    {
            //        course.WordSuites.Add(wordSuites[i++]);
            //        var wordsuitesToAdd1 = context.WordSuites.Where(ws => ws.PrototypeId == i);
            //        foreach (var wordsuite in wordsuitesToAdd1)
            //        {
            //            course.WordSuites.Add(wordsuite);
            //        }
            //        course.WordSuites.Add(wordSuites[i++]);
            //        var wordsuitesToAdd2 = context.WordSuites.Where(ws => ws.PrototypeId == i);
            //        foreach (var wordsuite in wordsuitesToAdd2)
            //        {
            //            course.WordSuites.Add(wordsuite);
            //        }
            //        course.WordSuites.Add(wordSuites[i++]);
            //        var wordsuitesToAdd3 = context.WordSuites.Where(ws => ws.PrototypeId == i);
            //        foreach (var wordsuite in wordsuitesToAdd3)
            //        {
            //            course.WordSuites.Add(wordsuite);
            //        } 
            //    }
            //}
            //context.SaveChanges();
            //Distribute roles
            var roles = context.Roles.ToList();
            foreach (var user in context.Users)
            {
                //user.Roles.Clear();
                user.Roles.Add(roles[0]);
                user.Roles.Add(roles[1]);
                user.Roles.Add(roles[2]);
            }
            //Assign words to wordsuites
            //foreach (var word in _dayWordModels)
            //{
            //    if (context.WordTranslations.Any(t => t.OriginalWordId == word.Id))
            //    {
            //        context.WordProgresses.AddOrUpdate(t => new { t.WordSuiteId, t.WordTranslationId }, new WordProgress
            //        {
            //            WordSuiteId = 1,
            //            Progress = 0,
            //            WordTranslationId = context.WordTranslations.First(t => t.OriginalWordId == word.Id).Id,
            //        });
            //    }
            //}
            //context.SaveChanges();
            //foreach (var word in _familyWordModels)
            //{
            //    if (context.WordTranslations.Any(t => t.OriginalWordId == word.Id))
            //    {
            //        context.WordProgresses.AddOrUpdate(t => new { t.WordSuiteId, t.WordTranslationId }, new WordProgress
            //        {
            //            WordSuiteId = 3,
            //            Progress = 0,
            //            WordTranslationId = context.WordTranslations.First(t => t.OriginalWordId == word.Id).Id
            //        });
            //    }
            //}
            //context.SaveChanges();
            //foreach (var word in _monthsWordModels)
            //{
            //    if (context.WordTranslations.Any(t => t.OriginalWordId == word.Id))
            //    {
            //        context.WordProgresses.AddOrUpdate(t => new { t.WordSuiteId, t.WordTranslationId }, new WordProgress
            //        {
            //            WordSuiteId = 2,
            //            Progress = 0,
            //            WordTranslationId = context.WordTranslations.First(t => t.OriginalWordId == word.Id).Id
            //        });
            //    }
            //}
            //context.SaveChanges();
            //foreach (var userId in _idOfUsersToAddWordProgresses)
            //{
            //    foreach (var wordSuiteId in _idOfWordSuitesToCopyWordProgresses)
            //    {
            //        CopyWordsFromWordSuiteForUser(context, userId, wordSuiteId);
            //    }
            //}
            context.SaveChanges();
        }

        //public String Sha256Hash(String value)
        //{
        //    using (SHA256 hash = SHA256.Create())
        //    {
        //        return String.Join("", hash
        //          .ComputeHash(Encoding.UTF8.GetBytes(value))
        //          .Select(item => item.ToString("x2")));
        //    }
        //}
    }
    //static class SeedHelper
    //{
    //    private static PropertyInfo[] PrimaryKeys<TEntity>()
    //        where TEntity : class
    //    {
    //        return typeof(TEntity).GetProperties()
    //                              .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))
    //                                       || "Id".Equals(p.Name, StringComparison.Ordinal))
    //                              .ToArray();
    //    }
    //    private static PropertyInfo[] Properties<TEntity>(
    //        Expression<Func<TEntity, object>> identifiers)
    //        where TEntity : class
    //    {
    //        var prop = identifiers.Body as MemberExpression;
    //        if (prop != null)
    //        {
    //            return new[] { (PropertyInfo)prop.Member };
    //        }

    //        var cons = identifiers.Body as NewExpression;
    //        if (cons != null)
    //        {
    //            return cons.Arguments
    //                       .Cast<MemberExpression>()
    //                       .Select(a => (PropertyInfo)a.Member)
    //                       .ToArray();
    //        }

    //        throw new NotSupportedException();
    //    }
    //    public static void AddOrUpdate<TEntity>(DbContext context,
    //        Expression<Func<TEntity, object>> identifiers,
    //        params TEntity[] entities)
    //        where TEntity : class
    //    {
    //        var primaryKeys = PrimaryKeys<TEntity>();
    //        var properties = Properties<TEntity>(identifiers);

    //        for (var i = 0; i < entities.Length; i++)
    //        {
    //            // build where condition for "identifiers"
    //            var parameter = Expression.Parameter(typeof(TEntity));
    //            var matches = properties.Select(p => Expression.Equal(
    //                Expression.Property(parameter, p),
    //                Expression.Constant(p.GetValue(entities[i]), p.PropertyType)));
    //            var match = Expression.Lambda<Func<TEntity, bool>>(
    //                matches.Aggregate((p, q) => Expression.AndAlso(p, q)),
    //                parameter);

    //            // match "identifiers" for current item
    //            var current = context.Set<TEntity>().SingleOrDefault(match);
    //            if (current != null)
    //            {
    //                // update primary keys
    //                foreach (var k in primaryKeys)
    //                    k.SetValue(entities[i], k.GetValue(current));

    //                // update all the values
    //                context.Entry(current).CurrentValues.SetValues(entities[i]);

    //                // replace updated item
    //                entities[i] = current;
    //            }
    //            else
    //            {
    //                // add new item
    //                context.Set<TEntity>().Add(entities[i]);
    //            }
    //            context.SaveChanges();
    //        }
    //    }
    //}
}