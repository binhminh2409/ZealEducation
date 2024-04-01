using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Models.ExamModule;

namespace ZealEducation.Models.Users
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set;}

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }

        public ICollection<Submission>? Submissions { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; } 
    }
}
