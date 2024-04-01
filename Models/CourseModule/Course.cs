using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.CandidateModule;

namespace ZealEducation.Models.CourseModule
{
    public class Course
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a course description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please assign a total number of sessions")]
        public int? TotalSessions { get; set; }

        [Required(ErrorMessage = "Please assign a course price")]
        public int? Price { get; set; }

        public ICollection<CourseSession>? Sessions { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
