using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.BatchModule
{
    [NotMapped]
    public class BatchDTO
    {
        [Required(ErrorMessage = "Please assign a Course Id")]
        public string? CourseId { get; set; }

        [Required(ErrorMessage = "Please assign number of candidates for the batch"), Range(11, 100, ErrorMessage = "Number of batch's candidates must be greater than 10")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Please assign a start date")]
        [StartDateValidation(ErrorMessage = "Start date must be in the future and before end date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please assign an end date")]
        public DateTime? EndDate { get; set; }
    }

    public class StartDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime?)value;
            var batchDTO = (BatchDTO)validationContext.ObjectInstance;

            if (startDate.HasValue && batchDTO.EndDate.HasValue && startDate >= batchDTO.EndDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
