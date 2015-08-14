using System.Collections.Generic;

namespace WorldOfWords.API.Models
{
    public class WordSuiteEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public int LanguageId { get; set; }
        public int Threshold { get; set; }
        public int QuizResponseTime { get; set; }
        public List<int> WordTranslationsToAddIdRange { get; set; }
        public List<int> WordTranslationsToDeleteIdRange { get; set; }
        public bool IsBasicInfoChanged { get; set; }
    }
}
