using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services.IServices
{
    public interface ILanguageService
    {
        int AddLanguage(Language language);
        bool RemoveLanguage(int id);
        List<Language> GetAllLanguages();
    }
}