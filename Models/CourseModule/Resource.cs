using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZealEducation.Models.CourseModule
{
    public class Resource
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course session id"), JsonIgnore]
        public CourseSession? CourseSession { get; set; }

        [Required(ErrorMessage = "Please assign a resource name")]
        public string? Name { get; set; }

        public string? FileName { get; set; }

        [Required(ErrorMessage = "Please assign a resource type")]
        public string? Type { get; set; }
    }
}
