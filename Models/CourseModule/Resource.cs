using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.CourseModule
{
    public class Resource
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course session id")]
        public CourseSession? CourseSession { get; set; }

        [Required(ErrorMessage = "Please assign a resource name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a resource file")]
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "Please assign a resource type")]
        public string? Type { get; set; }
    }
}
