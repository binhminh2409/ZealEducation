using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.Batch
{
    public class Batch
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please assign a course")]
        public string CourseId { get; set; }
        [Required(ErrorMessage = "Please assign number of candidates for the batch")]
        public string Quantity { get; set; }
        [Required(ErrorMessage = "Please assign a start date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please assign an end date")]
        public DateTime EndDate { get; set; }
        
        public ICollection<Exam> Exams { get; set; } 


    }
}
