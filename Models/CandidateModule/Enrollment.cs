using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.CandidateModule
{
    public class Enrollment
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course"), JsonIgnore]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Please assign an user id"), JsonIgnore]
        public UserInfo? UserInfo { get; set; }

        [Required(ErrorMessage = "Please assign a created date")]
        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage = "Please enter the transaction amount")]
        public int? Amount { get; set; }

        [Required(ErrorMessage = "Please assign a status")]
        public string? Status { get; set; }

        public string? Note { get; set; }
    }
}