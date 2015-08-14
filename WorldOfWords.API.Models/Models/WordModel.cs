namespace WorldOfWords.API.Models
{
    public class WordModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Transcription { get; set; }
        public int LanguageId { get; set; }
    }
}
