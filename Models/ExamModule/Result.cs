using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.ExamModule
{
    public class Result
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a submission id")]
        public Submission? Submission { get; set; }

        [Required(ErrorMessage = "Please assign a score")]
        public int? Score { get; set; }

        [Required(ErrorMessage = "Please decide passage")]
        public bool? Passed { get; set; }
    }
}
