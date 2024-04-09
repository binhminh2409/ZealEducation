using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.CandidateModule
{
    public class Enrollment
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course")]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Please assign an user id")]
        public UserInfo? UserInfo { get; set; }

        [Required(ErrorMessage = "Please assign a created date")]
        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage = "Please assign a status")]
        public string? Status { get; set; }
    }
}