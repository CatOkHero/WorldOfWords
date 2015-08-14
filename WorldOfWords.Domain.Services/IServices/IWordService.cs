using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services
{
    public interface IWordService
    {
        List<Word> GetAllWords();
        List<Word> GetAllWordsBySpecificLanguage(int languageId);
        List<Word> GetTopBySearchWord(string searchWord, int languageId, int count);
        Word GetWordById(int id);
        int Add(Word word);
        int Exists(string value, int languageId);
    }
}