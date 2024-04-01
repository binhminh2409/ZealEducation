using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.BatchModule
{
    public class BatchSession
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a batch id")]
        public Batch? Batch { get; set; }

        [Required(ErrorMessage = "Please assign a batch session name")]
        public string? Name { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}
