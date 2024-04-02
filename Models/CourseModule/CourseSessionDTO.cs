using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZealEducation.Models.CourseModule
{
    [NotMapped]
    public class CourseSessionDTO
    {
        [Required(ErrorMessage = "Please assign a course session name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a course session description")]
        public string? Description { get; set; }

        public ICollection<ResourceDTO>? Resources { get; set; }
    }
}
