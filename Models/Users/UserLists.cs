using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.Faculty
{
    [NotMapped]
    public class UserLists
    {
        public class User
        {
            public string? Username { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public List<string?> Roles { get; set; }
        }

        public List<User> Users { get; set; } = new List<User>();
    }
}
