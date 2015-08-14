using System.Collections.Generic;

namespace WorldOfWords.Domain.Models
{
    public partial class User
    {
        public User()
        {
            this.WordSuites=new List<WordSuite>();
            this.Roles = new List<Role>();
            this.Enrollments = new List<Enrollment>();
            this.Courses=new List<Course>();
            this.OwnedGroups=new List<Group>();
            this.OwnedWordTranslations=new List<WordTranslation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string HashedToken { get; set; }
        public virtual ICollection<WordSuite> WordSuites { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Group> OwnedGroups { get; set; }
        public virtual ICollection<WordTranslation> OwnedWordTranslations { get; set; }
    }
}
