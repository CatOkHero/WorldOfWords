using System.Collections.Generic;
using WorldOfWords.API.Models.Models;

namespace WorldOfWords.API.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Progress { get; set;}
        public LanguageModel Language { get; set; }
        public List<CourseWordSuiteModel> WordSuites { get; set; }
    }
}
