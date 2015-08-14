using System.Collections.Generic;

namespace WorldOfWords.Domain.Models
{
    public class Tag
    {
        public Tag()
        {
            WordTranslations = new List<WordTranslation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WordTranslation> WordTranslations { get; set; } 
    }
}
