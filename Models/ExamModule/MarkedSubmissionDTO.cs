using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.ExamModule
{
    [NotMapped]
    public class MarkedSubmissionDTO
    {
        [Required]
        public string? SubmissionId { get; set; }

        [Required]
        public int? Score { get; set; }
    }
}
