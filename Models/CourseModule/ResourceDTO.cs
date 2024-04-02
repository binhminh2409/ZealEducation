using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.CourseModule
{
    public class ResourceDTO
    {
        [Required(ErrorMessage = "Please assign a resource name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a resource file")]
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "Please assign a resource type")]
        public string? Type { get; set; }
    }
}