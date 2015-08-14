namespace WorldOfWords.API.Models
{
    public class UserForListOfUsersModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual System.Collections.Generic.ICollection<RoleModel> Roles { get; set; }
    }
}
