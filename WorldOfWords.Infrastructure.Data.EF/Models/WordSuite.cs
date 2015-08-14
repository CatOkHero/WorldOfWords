using System.Collections.Generic;

namespace WorldOfWords.Infrastructure.Data.EF.Models
{
    public partial class WordSuite
    {
        public WordSuite()
        {
            this.WordProgress = new List<WordProgress>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }
        public int ReviewThreshold { get; set; }
        public int OwnerId { get; set; }
        public virtual Language Language { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<WordProgress> WordProgress { get; set; }
    }
}
