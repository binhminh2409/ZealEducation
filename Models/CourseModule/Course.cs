using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZealEducation.Models.CandidateModule;

namespace ZealEducation.Models.CourseModule
{
    public class Course
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please assign a course name")]
        public string? Name { get; set; }

        public string? ImageName { get; set; }

        [Required(ErrorMessage = "Please assign a course description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please assign a total number of sessions")]
        public int? TotalSessions { get; set; }

        [Required(ErrorMessage = "Please assign a course price"), Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public int? Price { get; set; }

        public ICollection<CourseSession>? Sessions { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
