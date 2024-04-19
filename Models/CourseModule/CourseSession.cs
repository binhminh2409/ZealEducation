using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZealEducation.Models.CourseModule
{
    public class CourseSession
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course id"), JsonIgnore]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Please assign a course session name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a course session description")]
        public string? Description { get; set; }

        public ICollection<Resource>? Resources { get; set; }
    }
}
