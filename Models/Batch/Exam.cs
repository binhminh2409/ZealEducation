using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.Batch
{
    public class Exam
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide exam name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide exam start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please provide exam end date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please provide batch")]
        public int BatchId { get; set; }
    }
}