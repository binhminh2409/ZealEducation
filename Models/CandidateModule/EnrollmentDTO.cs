using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.CandidateModule
{
    [NotMapped]
    public class EnrollmentDTO
    {
        [Required(ErrorMessage = "Please assign a course")]
        public string? CourseId { get; set; }

        [Required(ErrorMessage = "Please enter the transaction amount")]
        public int? Amount { get; set; }

        [Required(ErrorMessage = "Please assign a status")]
        public string? Status { get; set; }

        public string? Note { get; set; }
    }
}
