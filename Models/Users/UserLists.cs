using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.Users
{
    [NotMapped]
    public class UserList
    {
        public class User
        {
            public string? Username { get; set; }
            public string? Email { get; set; }
            public List<string?>? Roles { get; set; }
        }

        public List<User> Users { get; set; } = new List<User>();
    }
}
