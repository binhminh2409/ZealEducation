using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Models.ExamModule;

namespace ZealEducation.Models.Users
{
    public class User : IdentityUser
    {
        public UserInfo? UserInfo { get; set; }
    }
}
