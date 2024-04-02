using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZealEducation.Models.CandidateModule;

namespace ZealEducation.Models.CourseModule
{
    [NotMapped]
    public class CourseDTO
    {
        [Required(ErrorMessage = "Please assign a course name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please assign a course description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please assign a course price"), Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public int? Price { get; set; }

        public ICollection<CourseSessionDTO>? Sessions { get; set; }
    }
}
