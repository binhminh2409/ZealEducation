using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZealEducation.Models.CourseModule;

namespace ZealEducation.Models.BatchModule
{
    public class BatchSession
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a batch id"), JsonIgnore]
        public Batch? Batch { get; set; }

        [Required(ErrorMessage = "Please assign a batch session name")]
        public string? Name { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}
