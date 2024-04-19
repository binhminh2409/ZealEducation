using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.ExamModule
{
    [NotMapped]
    public class ExamDTO
    {
        [Required(ErrorMessage = "Please provide the batch id")]
        public string? batchId { get; set; }

        [Required(ErrorMessage = "Please provide exam name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please provide exam start date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please provide exam end date")]
        public DateTime? EndDate { get; set; }


        [Required(ErrorMessage = "Please provide exam passing score")]
        public int? ScoreToPass { get; set; }

    }
}