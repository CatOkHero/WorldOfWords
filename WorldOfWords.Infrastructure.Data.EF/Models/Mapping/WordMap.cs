using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WorldOfWords.Infrastructure.Data.EF.Models.Mapping
{
    public class WordMap : EntityTypeConfiguration<Word>
    {
        public WordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Words");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Transcription).HasColumnName("Transcription");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.WordSuite_Id).HasColumnName("WordSuite_Id");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.Words)
                .HasForeignKey(d => d.LanguageId);
         

        }
    }
}
