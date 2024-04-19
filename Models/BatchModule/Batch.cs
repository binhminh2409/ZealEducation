using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.ExamModule;

namespace ZealEducation.Models.BatchModule
{
    public class Batch
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a course")]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Please assign number of candidates for the batch")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Please assign a start date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please assign an end date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please assign a total number of sessions")]
        public int? TotalSessions { get; set; }

        public ICollection<Exam>? Exams { get; set; }

        public ICollection<BatchSession>? BatchSessions { get; set; }

    }
}
