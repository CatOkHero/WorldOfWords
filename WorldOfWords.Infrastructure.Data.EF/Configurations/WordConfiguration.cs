using System.Data.Entity.ModelConfiguration;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Infrastructure.Data.EF.Configurations
{
    public class WordConfiguration : EntityTypeConfiguration<Word>
    {
        public WordConfiguration()
        {
            //Primary Key
            HasKey(t => t.Id);

            //Properties
            ToTable("Words");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Value).HasColumnName("Value").IsRequired();
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.Transcription).HasColumnName("Transcription");
            Property(t => t.LanguageId).HasColumnName("LanguageId");

            //Table & Column Mappings
            HasRequired(t => t.Language)
                .WithMany(t => t.Words)
                .HasForeignKey(t => t.LanguageId);
            HasMany(t => t.WordTranslationsAsWord)
                .WithRequired(t => t.OriginalWord);
            HasMany(t => t.WordTranslationsAsTranslation)
                .WithRequired(t => t.TranslationWord);
        }
    }
}
