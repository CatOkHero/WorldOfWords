using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WorldOfWords.Infrastructure.Data.EF.Models.Mapping
{
    public class WordSuiteMap : EntityTypeConfiguration<WordSuite>
    {
        public WordSuiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WordSuites");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            //this.Property(t => t.WrongReviewPenalty).HasColumnName("WrongReviewPenalty");
            this.Property(t => t.OwnerId).HasColumnName("OwnerId");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.WordSuites)
                .HasForeignKey(d => d.LanguageId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.WordSuites)
                .HasForeignKey(d => d.OwnerId);

        }
    }
}
