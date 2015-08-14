using System.Collections.Generic;

namespace WorldOfWords.Domain.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Transcription { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<WordTranslation> WordTranslationsAsWord { get; set; }
        public virtual ICollection<WordTranslation> WordTranslationsAsTranslation { get; set; } 
    }
}
