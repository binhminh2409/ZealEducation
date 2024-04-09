using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Models.ExamModule;

namespace ZealEducation.Models.Users
{
    public class UserInfo
    {
        [Key] public string Id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [JsonIgnore]
        public ICollection<Attendance>? Attendances { get; set; }

        [JsonIgnore]
        public ICollection<Submission>? Submissions { get; set; }

        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments { get; set; }
    }

}
