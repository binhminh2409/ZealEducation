using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.Users
{
    [NotMapped]
    public class UserInfoList
    {
        public class UserInfo
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
        }

        public List<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
    }
}
