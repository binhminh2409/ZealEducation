using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.BatchModule;

namespace ZealEducation.Models.ExamModule
{
    public class Exam
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide batch")]
        public Batch? Batch { get; set; }

        [Required(ErrorMessage = "Please provide exam name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please provide exam start date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please provide exam end date")]
        public DateTime? EndDate { get; set; }

        public ICollection<Submission>? Submissions { get; set; }
    }
}